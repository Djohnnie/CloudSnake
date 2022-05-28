using CloudSnake.Domain;
using Microsoft.EntityFrameworkCore;

namespace CloudSnake.DataAccess;

public class CloudSnakeDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Player> Players { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\SQLDEV;Database=CloudSnake;Trusted_Connection=True;");
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