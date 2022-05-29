using CloudSnake.DataAccess;
using CloudSnake.Domain;
using CloudSnake.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CloudSnake.SignalR.Hubs;

public class TickerHub : Hub
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TickerHub(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task SendGameState(GameState gameState)
    {
        if (Clients != null)
        {
            await Clients.Group(gameState.GameCode).SendAsync("ReceiveGameState", gameState);
        }
    }

    public async Task JoinGame(string gameCode)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameCode);
    }

    public async Task Turn(string gameCode, string playerName, Orientation orientation)
    {
        if (string.IsNullOrWhiteSpace(gameCode) || string.IsNullOrWhiteSpace(playerName))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<CloudSnakeDbContext>();
        var player = await dbContext.Players.SingleOrDefaultAsync(x => x.Game.Code == gameCode && x.Name == playerName);
        if (player != null)
        {
            var snake = Snake.FromSnakeData(player.SnakeData);
            snake.Orientation = orientation;
            player.SnakeData = snake.ToSnakeData();
            await dbContext.SaveChangesAsync();
        }
    }
}