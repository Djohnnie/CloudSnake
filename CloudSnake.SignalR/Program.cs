using CloudSnake.SignalR.Hubs;
using CloudSnake.SignalR.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddHostedService<TickerWorker>();
builder.Services.AddSingleton<TickerHub>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapHub<TickerHub>("/ticker");

app.Run();