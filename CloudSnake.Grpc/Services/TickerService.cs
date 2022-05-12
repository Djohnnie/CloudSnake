using System.Collections.Concurrent;
using Grpc.Core;

namespace CloudSnake.Grpc.Services;

public class TickerService : Ticker.TickerBase
{
    private static readonly ConcurrentDictionary<string, IServerStreamWriter<TickerResponse>> _subscribers = new ConcurrentDictionary<string, IServerStreamWriter<TickerResponse>>();

    private readonly ILogger<TickerService> _logger;

    public TickerService(ILogger<TickerService> logger)
    {
        _logger = logger;
    }

    public override async Task Do(
        TickerRequest request,
        IServerStreamWriter<TickerResponse> responseStream,
        ServerCallContext context)
    {
        _subscribers.TryAdd(request.ClientId, responseStream);

        while (!context.CancellationToken.IsCancellationRequested)
        {
            var response = new TickerResponse
            {

            };

            foreach (var subscriber in _subscribers)
            {
                await subscriber.Value.WriteAsync(response);
            }

            await Task.Delay(1000);
        }
    }
}