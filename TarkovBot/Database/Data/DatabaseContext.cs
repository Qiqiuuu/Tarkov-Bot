using Microsoft.EntityFrameworkCore;
using TarkovBot.Database.Modules;

namespace TarkovBot.Database.Data;

public class DatabaseContext:DbContext
{
    public DbSet<TeamKill> TeamKills { get; set; }
    public DbSet<TwitterPost> TwitterPosts { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamKill>(e =>
        {
            e.ToTable("TeamKills");
            e.HasKey(x => x.Id);
            e.Property(x => x.GuildId).IsRequired();
            e.Property(x => x.KillerId).IsRequired();
            e.Property(x => x.KilledId).IsRequired();
            e.Property(x => x.Date);
            e.Property(x => x.Description);
        });

        modelBuilder.Entity<TwitterPost>(e =>
        {
            e.ToTable("TwitterPosts");
            e.HasKey(x => x.Id);
            e.Property(x => x.ChannelId).IsRequired();
            e.Property(x => x.AccountId).IsRequired();
            e.Property(x => x.LastTweetId);
        });
        
        
        base.OnModelCreating(modelBuilder);
    }
}