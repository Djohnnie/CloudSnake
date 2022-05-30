using CloudSnake.DataAccess;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CloudSnake.FunctionApp.Cleanup;

public class CleanupFunction
{
    private readonly CloudSnakeDbContext _dbContext;

    public CleanupFunction(CloudSnakeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [FunctionName("CleanupFunction")]
    public async Task Run([TimerTrigger("0 0/5 * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"CleanupFunction (time triggered) executing at: {DateTime.Now}");

        var inactiveGames = await _dbContext.Games.Where(x => !x.IsActive).ToListAsync();

        var inactiveGameCount = inactiveGames.Count;
        var inactivePlayerCount = 0;

        foreach (var game in inactiveGames)
        {
            var inactivePlayers = await _dbContext.Players.Where(x => x.Game.Code == game.Code).ToListAsync();
            inactivePlayerCount += inactivePlayers.Count;
            _dbContext.Players.RemoveRange(inactivePlayers);
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }

        log.LogInformation($"{inactiveGameCount} games and {inactivePlayerCount} players have been cleaned!");
    }
}