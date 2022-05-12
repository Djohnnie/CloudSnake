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

    public void Register(string clientId)
    {

    }

    public void Ready(string clientId)
    {

    }

    public async Task Turn(string clientId)
    {
        _clientIds.Add(clientId);
    }
}