using CloudSnake.Business;
using CloudSnake.Dto;
using CloudSnake.SignalR.Hubs;

namespace CloudSnake.SignalR.Workers;

public class TickerWorker : BackgroundService
{
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
            using var serviceScope = _serviceScopeFactory.CreateScope();
            var gameManager = serviceScope.ServiceProvider.GetService<GameManager>();
            var activeGames = await gameManager.GetActiveGames();

            foreach (var activeGame in activeGames.ActiveGames)
            {
                var gameState = new GameState(
                    activeGame.GameCode,
                    activeGame.Players.Select(x => new PlayerState(x.PlayerName, x.IsReady)).ToList());

                await _tickerHub.SendGameState(gameState);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}