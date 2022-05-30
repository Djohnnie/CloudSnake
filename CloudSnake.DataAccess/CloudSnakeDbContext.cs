using CloudSnake.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CloudSnake.DataAccess;

public class CloudSnakeDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }

    public CloudSnakeDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetValue<string>("CONNECTION_STRING");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(e =>
        {
            e.ToTable("GAMES");
            e.HasKey(x => x.Id).IsClustered(false);
            e.HasIndex(x => x.SysId).IsClustered();
            e.Property(x => x.SysId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Player>(e =>
        {
            e.ToTable("PLAYERS");
            e.HasKey(x => x.Id).IsClustered(false);
            e.HasIndex(x => x.SysId).IsClustered();
            e.Property(x => x.SysId).ValueGeneratedOnAdd();
        });
    }
}