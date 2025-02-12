namespace DungeonDates.Shared.Features.Responses;

public class PostDetailRequest
{
    public required Guid ProposedDateId { get; set; }
    
    public required string Name { get; init; }
    public required bool Accepted { get; init; }
}