namespace DungeonDates.Shared.Features.Responses;

public class GetDetailRequest
{
    public required Guid Id { get; set; }
}

public class GetDetailResponse
{
    public required Guid Id { get; init; }

    public required IEnumerable<ProposedDate> ProposedDates { get; init; } = [];

    public bool NoDatesWork => ProposedDates.All(pd => pd.TotalResponseCount != pd.AcceptedResponseCount);
    
    public class ProposedDate
    {
        public required Guid Id { get; init; }
        
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }

        public required IEnumerable<ProposedDateResponse> Responses { get; init; } = [];
        
        public int TotalResponseCount => Responses.Count();
        public int AcceptedResponseCount => Responses.Count(response => response.Accepted);

        public class ProposedDateResponse
        {
            public required string Name { get; init; }
            public required bool Accepted { get; init; }
        }
    }
}