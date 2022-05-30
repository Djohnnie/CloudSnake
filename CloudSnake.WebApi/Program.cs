using CloudSnake.Business;
using CloudSnake.Business.Helpers;
using CloudSnake.DataAccess;
using CloudSnake.Dto;
using CloudSnake.WebApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<GameCodeHelper>();
builder.Services.AddDbContext<CloudSnakeDbContext>();
builder.Services.AddTransient<GameManager>();
builder.Services.AddTransient(typeof(IApiHelper<>), typeof(ApiHelper<>));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/status", () => Results.Ok);
app.MapPost("/games", (IApiHelper<GameManager> helper, CreateGameRequest request) => helper.Post(l => l.CreateGame(request)));
app.MapPost("/games/{gameCode}/join", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, JoinGameRequest request) => helper.Post(l => l.JoinGame(request with { GameCode = gameCode })));
app.MapPost("/games/{gameCode}/ready/{playerName}", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.ReadyPlayer(new ReadyPlayerRequest(gameCode, playerName))));
app.MapPost("/games/{gameCode}/abandon/{playerName}", (IApiHelper<GameManager> helper, [FromRoute] string gameCode, [FromRoute] string playerName) => helper.Post(l => l.Abandon(new AbandonRequest(gameCode, playerName))));
app.MapGet("/games/active", (IApiHelper<GameManager> helper) => helper.Execute(l => l.GetActiveGames()));

app.Run();