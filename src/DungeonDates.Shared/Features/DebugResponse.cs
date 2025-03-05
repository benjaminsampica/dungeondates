namespace DungeonDates.Shared.Features;

public class DebugResponse
{
    public List<DebugModel> Data { get; set; } = [];
    
    public class DebugModel
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }
}