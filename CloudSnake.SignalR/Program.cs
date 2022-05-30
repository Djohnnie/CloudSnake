using CloudSnake.Business;
using CloudSnake.Business.Helpers;
using CloudSnake.DataAccess;
using CloudSnake.SignalR.Hubs;
using CloudSnake.SignalR.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<GameCodeHelper>();
builder.Services.AddDbContext<CloudSnakeDbContext>();
builder.Services.AddTransient<GameManager>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<TickerWorker>();
builder.Services.AddSingleton<TickerHub>();
var app = builder.Build();

app.MapGet("/", () => "This is a SignalR host application!");

app.MapHub<TickerHub>("/ticker");

app.Run();