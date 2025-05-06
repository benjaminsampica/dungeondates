namespace DungeonDates.Function.Infrastructure.Databases;

public class DungeonDate
{
    public string Id { get; set; } = Guid.CreateVersion7().ToString();
    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;
    public required ICollection<ProposedDate> Dates { get; init; } = [];
}

public class ProposedDate
{
    public Guid Id { get; init; }
    public required DateTime StartDate { get; init; }
    public required DateTime EndDate { get; init; }
    
    public ICollection<ProposedDateResponse> Responses { get; init; } = [];
}

public class ProposedDateResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool Accepted { get; init; }
    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;
}