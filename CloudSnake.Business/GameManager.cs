using CloudSnake.Business.Helpers;
using CloudSnake.DataAccess;
using CloudSnake.Domain;
using CloudSnake.Dto;
using Microsoft.EntityFrameworkCore;

namespace CloudSnake.Business;

public class GameManager
{
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
            IsActive = true
        };

        var player = new Player
        {
            Id = Guid.NewGuid(),
            Game = game,
            Name = request.HostPlayerName
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
            Name = request.PlayerName
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

        return null;
    }

    public async Task<GetActiveGamesResponse> GetActiveGames()
    {
        var activeGames = await _cloudSnakeDbContext.Games
            .Include(x => x.Players)
            .Where(x => x.IsActive).ToListAsync();

        return new GetActiveGamesResponse(
            activeGames.Select(x => new ActiveGame(x.Code, x.Players.Select(p => new ActivePlayer(p.Name, p.IsReady)).ToList())).ToList());
    }
}