using CloudSnake.Business.Helpers;
using CloudSnake.DataAccess;
using CloudSnake.Domain;
using CloudSnake.Dto;
using Microsoft.EntityFrameworkCore;

namespace CloudSnake.Business;

public class GameManager
{
    public const int SnakeGameWidth = 30;
    public const int SnakeGameHeight = 16;
    public const int SnakeLength = 5;

    private readonly CloudSnakeDbContext _cloudSnakeDbContext;
    private readonly GameCodeHelper _gameCodeHelper;

    public GameManager(
        CloudSnakeDbContext cloudSnakeDbContext,
        GameCodeHelper gameCodeHelper)
    {
        _cloudSnakeDbContext = cloudSnakeDbContext;
        _gameCodeHelper = gameCodeHelper;
    }

    public async Task<CreateGameResponse> CreateGame(CreateGameRequest request)
    {
        var gameCode = string.Empty;

        do
        {
            gameCode = _gameCodeHelper.GenerateGameCode();
        }
        while (await _cloudSnakeDbContext.Games.AnyAsync(x => x.Code == gameCode));

        var game = new Game
        {
            Id = Guid.NewGuid(),
            Code = gameCode,
            IsActive = true,
            FoodData = new Food { Bites = new List<Bite>() }.ToFoodData()
        };

        var player = new Player
        {
            Id = Guid.NewGuid(),
            Game = game,
            Name = request.HostPlayerName,
            SnakeData = Snake.RandomSnake(SnakeGameWidth, SnakeGameHeight, SnakeLength).ToSnakeData()
        };

        _cloudSnakeDbContext.Games.Add(game);
        _cloudSnakeDbContext.Players.Add(player);
        await _cloudSnakeDbContext.SaveChangesAsync();

        return new CreateGameResponse(gameCode);
    }

    public async Task<JoinGameResponse> JoinGame(JoinGameRequest request)
    {
        var game = await _cloudSnakeDbContext.Games.SingleOrDefaultAsync(x => x.Code == request.GameCode);

        if (game == null)
        {
            return null;
        }

        var player = new Player
        {
            Id = Guid.NewGuid(),
            Game = game,
            Name = request.PlayerName,
            SnakeData = Snake.RandomSnake(SnakeGameWidth, SnakeGameHeight, SnakeLength).ToSnakeData()
        };

        _cloudSnakeDbContext.Players.Add(player);
        await _cloudSnakeDbContext.SaveChangesAsync();

        return new JoinGameResponse(game.Code);
    }

    public async Task<ReadyPlayerResponse> ReadyPlayer(ReadyPlayerRequest request)
    {
        var player = await _cloudSnakeDbContext.Players.SingleOrDefaultAsync(x => x.Game.Code == request.GameCode && x.Name == request.PlayerName);

        if (player != null)
        {
            player.IsReady = true;
            await _cloudSnakeDbContext.SaveChangesAsync();
        }

        var players = await _cloudSnakeDbContext.Players.Where(x => x.Game.Code == request.GameCode).ToListAsync();

        if (players.All(x => x.IsReady))
        {
            await ReadyGame(request.GameCode);
        }

        return new ReadyPlayerResponse();
    }

    public async Task<GetActiveGamesResponse> GetActiveGames()
    {
        var activeGames = await _cloudSnakeDbContext.Games
            .Include(x => x.Players)
            .Where(x => x.IsActive).ToListAsync();

        return new GetActiveGamesResponse(
            activeGames.Select(x => new ActiveGame(x.Code, x.IsReady, x.Players.Select(p => new ActivePlayer(p.Name, p.IsReady, p.SnakeData)).ToList(), x.FoodData)).ToList());
    }

    public async Task<Orientation> GetPlayerOrientation(string gameCode, string playerName, Orientation current)
    {
        var player = await _cloudSnakeDbContext.Players.SingleOrDefaultAsync(x => x.Game.Code == gameCode && x.Name == playerName);

        if (player != null)
        {
            return Snake.FromSnakeData(player.SnakeData).Orientation;
        }

        return current;
    }

    public async Task<AbandonResponse> Abandon(AbandonRequest request)
    {
        var player = await _cloudSnakeDbContext.Players.SingleOrDefaultAsync(x => x.Game.Code == request.GameCode && x.Name == request.PlayerName);

        if (player != null)
        {
            player.IsReady = false;
            await _cloudSnakeDbContext.SaveChangesAsync();
        }

        var players = await _cloudSnakeDbContext.Players.Where(x => x.Game.Code == request.GameCode).ToListAsync();
        if (players.All(x => !x.IsReady))
        {
            var game = await _cloudSnakeDbContext.Games.SingleOrDefaultAsync(x => x.Code == request.GameCode);
            game.IsActive = false;
            await _cloudSnakeDbContext.SaveChangesAsync();
        }

        return new AbandonResponse();
    }

    public async Task ReadyGame(string gameCode)
    {
        var game = await _cloudSnakeDbContext.Games.SingleOrDefaultAsync(x => x.Code == gameCode);

        if (game != null)
        {
            game.IsReady = true;
            await _cloudSnakeDbContext.SaveChangesAsync();
        }
    }

    public async Task UpdatePlayerStates(ActiveGame activeGame, List<PlayerState> playerStates)
    {
        var players = await _cloudSnakeDbContext.Players.Where(x => x.Game.Code == activeGame.GameCode).ToListAsync();

        foreach (var player in players)
        {
            var newPlayerState = playerStates.SingleOrDefault(x => x.PlayerName == player.Name);
            if (newPlayerState != null)
            {
                player.SnakeData = newPlayerState.Snake.ToSnakeData();
            }
        }

        await _cloudSnakeDbContext.SaveChangesAsync();
    }

    public async Task UpdateFood(ActiveGame activeGame, Food food)
    {
        var game = await _cloudSnakeDbContext.Games.SingleOrDefaultAsync(x => x.Code == activeGame.GameCode);

        if (game != null)
        {
            game.FoodData = food.ToFoodData();
            await _cloudSnakeDbContext.SaveChangesAsync();
        }
    }
}