using System.Diagnostics;
using CloudSnake.Business;
using CloudSnake.Domain;
using CloudSnake.Dto;
using CloudSnake.SignalR.Hubs;

namespace CloudSnake.SignalR.Workers;

public class TickerWorker : BackgroundService
{
    private readonly Random _random = new();
    private readonly TickerHub _tickerHub;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TickerWorker(
        TickerHub tickerHub,
        IServiceScopeFactory serviceScopeFactory)
    {
        _tickerHub = tickerHub;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var stopWatch = Stopwatch.StartNew();

            using var serviceScope = _serviceScopeFactory.CreateScope();
            var gameManager = serviceScope.ServiceProvider.GetService<GameManager>();
            var activeGames = await gameManager.GetActiveGames();

            foreach (var activeGame in activeGames.ActiveGames)
            {
                var isReady = activeGame.IsReady || activeGame.Players.All(x => x.IsReady);

                var gameState = new GameState(
                    GameManager.SnakeGameWidth, GameManager.SnakeGameHeight, isReady, activeGame.GameCode,
                    activeGame.Players.Select(x => new PlayerState(x.PlayerName, x.IsReady, Snake.FromSnakeData(x.SnakeData))).ToList(),
                    Food.FromFoodData(activeGame.FoodData));

                if (isReady)
                {
                    var food = Food.FromFoodData(activeGame.FoodData);

                    if (food.Bites.Count == 0)
                    {
                        food = GenerateRandomFood(activeGame, food);
                        await gameManager.UpdateFood(activeGame, food);
                    }

                    var newPlayerStates = new List<PlayerState>();

                    foreach (var playerState in gameState.Players)
                    {
                        bool shouldGrow = false;
                        var bitesToRemove = new List<Bite>();

                        if (food.Bites.Count > 0)
                        {
                            foreach (var bite in food.Bites)
                            {
                                foreach (var coordinate in playerState.Snake.Coordinates)
                                {
                                    if (bite.X == coordinate.X && bite.Y == coordinate.Y)
                                    {
                                        shouldGrow = true;
                                        bitesToRemove.Add(bite);
                                        break;
                                    }
                                }
                            }
                        }

                        if (shouldGrow)
                        {
                            foreach (var biteToRemove in bitesToRemove)
                            {
                                food.Bites.Remove(biteToRemove);
                            }
                        }

                        var lastMinuteOrientation = await gameManager.GetPlayerOrientation(activeGame.GameCode, playerState.PlayerName, playerState.Snake.Orientation);
                        var newPlayerState = playerState with { Snake = playerState.Snake with { Orientation = lastMinuteOrientation } };
                        var newState = ProgressSnake(newPlayerState, shouldGrow);
                        newPlayerStates.Add(newState);
                    }

                    await gameManager.UpdateFood(activeGame, food);
                    await gameManager.UpdatePlayerStates(activeGame, newPlayerStates);
                    gameState = gameState with { Players = newPlayerStates, Food = food };
                }

                await _tickerHub.SendGameState(gameState);
            }

            stopWatch.Stop();
            await Task.Delay(Math.Max(0, 500 - (int)stopWatch.ElapsedMilliseconds), stoppingToken);
        }
    }

    private Food GenerateRandomFood(ActiveGame activeGame, Food food)
    {
        bool collision = false;

        do
        {
            var randomX = _random.Next(0, GameManager.SnakeGameWidth);
            var randomY = _random.Next(0, GameManager.SnakeGameHeight);

            foreach (var bite in food.Bites)
            {
                if (randomX == bite.X && randomY == bite.Y)
                {
                    collision = true;
                    break;
                }
            }

            foreach (var player in activeGame.Players)
            {
                var snake = Snake.FromSnakeData(player.SnakeData);

                foreach (var coordinate in snake.Coordinates)
                {
                    if (randomX == coordinate.X && randomY == coordinate.Y)
                    {
                        collision = true;
                        break;
                    }
                }
            }

            if (!collision)
            {
                food.Bites.Add(new Bite { X = randomX, Y = randomY });
            }

        } while (collision);

        return food;
    }

    private PlayerState ProgressSnake(PlayerState playerState, bool shouldGrow)
    {
        var snake = playerState.Snake;

        Func<Coordinates, Coordinates> moveFunc = coordinates => coordinates;

        switch (snake.Orientation)
        {
            case Orientation.North:
                moveFunc = coordinates => coordinates with { Y = coordinates.Y - 1 };
                break;
            case Orientation.East:
                moveFunc = coordinates => coordinates with { X = coordinates.X + 1 };
                break;
            case Orientation.South:
                moveFunc = coordinates => coordinates with { Y = coordinates.Y + 1 };
                break;
            case Orientation.West:
                moveFunc = coordinates => coordinates with { X = coordinates.X - 1 };
                break;
        }

        var newCoordinates = new List<Coordinates>(snake.Coordinates.Count + (shouldGrow ? 1 : 0));

        for (int i = 0; i < snake.Coordinates.Count; i++)
        {
            if (i == 0)
            {
                newCoordinates.Add(moveFunc(snake.Coordinates[0]));
            }
            else
            {
                newCoordinates.Add(snake.Coordinates[i - 1]);
            }
        }

        if (shouldGrow)
        {
            newCoordinates.Add(snake.Coordinates[^1]);
        }

        return playerState with { Snake = snake with { Coordinates = newCoordinates } };
    }
}