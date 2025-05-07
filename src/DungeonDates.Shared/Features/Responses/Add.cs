namespace DungeonDates.Shared.Features.Responses;

public class PostDetailRequest
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    public List<ProposedDate> ProposedDateResponses { get; set; } = [];

    public class ProposedDate
    {
        public Guid ProposedDateId { get; set; }
        public bool Accepted { get; set; }
    }
}