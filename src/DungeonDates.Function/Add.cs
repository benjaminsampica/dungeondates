using Azure;
using Azure.Data.Tables;
using DungeonDates.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using System.Text.Json;

namespace DungeonDates.Function;

public class Add(IAzureClientFactory<TableServiceClient> clientFactory)
{
    private readonly TableClient _client = clientFactory.CreateClient("table").GetTableClient(nameof(DungeonDate));
    
    [Function("add")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous,  "post")] HttpRequest req)
    {
        var calendarDates = await JsonSerializer.DeserializeAsync<List<AddCalendarDate>>(req.Body, JsonSerializerOptions.Web) ?? [];

        if (calendarDates.Count == 0)
            return new BadRequestResult();

        var dateRanges = calendarDates.Select(cd => new DungeonDate.DateRange { Start = cd.Start, End = cd.End });
        var jsonListOfDateRanges = JsonSerializer.Serialize(dateRanges);

        var dungeonDate = new DungeonDate
        {
            JsonListOfDateRanges = jsonListOfDateRanges
        };
        await _client.AddEntityAsync(dungeonDate);
        
        return new OkObjectResult(new
        {
            Id = dungeonDate.RowKey
        });
    }
}

public class DungeonDate : ITableEntity
{
    public string PartitionKey { get; set; } = nameof(DungeonDate);
    public string RowKey { get; set; } = Guid.CreateVersion7().ToString();
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    
    public required string JsonListOfDateRanges { get; set; }

    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}