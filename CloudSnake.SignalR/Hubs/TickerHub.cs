using CloudSnake.Dto;
using Microsoft.AspNetCore.SignalR;

namespace CloudSnake.SignalR.Hubs;

public class TickerHub : Hub
{
    private List<string> _clientIds = new List<string>();

    public async Task SendMessage(string user, string message)
    {
        if (Clients != null)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
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

    public void Ready(string clientId)
    {

    }

    public async Task Turn(string clientId)
    {
        _clientIds.Add(clientId);
    }
}