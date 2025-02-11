using System.ComponentModel.DataAnnotations;

namespace DungeonDates.Function.Infrastructure.Databases;

public class DungeonDate
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;
    
    public required ICollection<ProposedDate> Dates { get; init; } = [];
}

public class ProposedDate
{
    public Guid Id { get; init; }
    public Guid DungeonDateId { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }

    public DungeonDate DungeonDate { get; init; } = null!;
    public ICollection<ProposedDateResponse> Responses { get; init; } = [];
}

public class ProposedDateResponse
{
    public Guid Id { get; init; }
    [StringLength(100)]
    public required string Name { get; init; }
    public required bool Accepted { get; init; }
    public Guid ProposedDateId { get; init; }
    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;
}