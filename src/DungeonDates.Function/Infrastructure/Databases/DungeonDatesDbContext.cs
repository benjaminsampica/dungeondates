using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DungeonDates.Function.Infrastructure.Databases;

public class DungeonDatesDbContext(DbContextOptions<DungeonDatesDbContext> options) : DbContext(options)
{
    public DbSet<DungeonDate> DungeonDates { get; init; }
    public DbSet<ProposedDate> ProposedDates { get; init; }
    public DbSet<ProposedDateResponse> ProposedDateResponses { get; init; }
}

public class DungeonDatesDbContextFactory : IDesignTimeDbContextFactory<DungeonDatesDbContext>
{
    public DungeonDatesDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DungeonDatesDbContext>();
        builder.UseSqlServer();
        
        return new(builder.Options);
    }
}