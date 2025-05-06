using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace DungeonDates.Function.Infrastructure.Databases;

public class DungeonDatesDbContext(DbContextOptions<DungeonDatesDbContext> options) : DbContext(options)
{
    public DbSet<DungeonDate> DungeonDates { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultContainer("DungeonDates");

        modelBuilder.Entity<DungeonDate>(builder =>
        {
            builder
                .HasDefaultTimeToLive(8_640_000) // 100 days.
                .UseETagConcurrency(); 
            
            builder.HasKey(d => d.Id);
            builder.HasPartitionKey(d => d.Id);
            builder.Property(d => d.Id).ToJsonProperty("id");
            builder.HasNoDiscriminator();
        });
    }
}