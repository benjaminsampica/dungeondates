using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace DungeonDates.Function;

public class Add
{
    [Function("add")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous,  "get")] HttpRequest req)
    {
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

public class DungeonDate : ITableEntity
{
    public string PartitionKey { get; set; } = nameof(DungeonDate);
    public string RowKey { get; set; } = Guid.CreateVersion7().ToString();
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    
    public DateTimeOffset? Date { get; set; }
}