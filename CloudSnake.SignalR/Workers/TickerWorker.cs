using CloudSnake.SignalR.Hubs;

namespace CloudSnake.SignalR.Workers;

public class TickerWorker : BackgroundService
{
    private readonly TickerHub _tickerHub;

    public TickerWorker(TickerHub tickerHub)
    {
        _tickerHub = tickerHub;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _tickerHub.SendMessage("test", "message");

            await Task.Delay(1000, stoppingToken);
        }
    }
}