using DungeonDates.Shared.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;

namespace DungeonDates.Function.Features;

public class Debug(IConfiguration configuration)
{
    [Function("debug")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous,  "get")] HttpRequest req)
    {
        var envVariables = Environment.GetEnvironmentVariables();
        var response = new DebugResponse();

        foreach (System.Collections.DictionaryEntry entry in envVariables)
        {
            response.Data.Add(new() { Key = entry.Key.ToString(), Value = entry.Value?.ToString() ?? ""});
        }
        
        foreach (var entry in configuration.AsEnumerable())
        {
            response.Data.Add(new() { Key = entry.Key, Value = entry.Value ?? ""});
        }

        return new OkObjectResult(response);
    }
}