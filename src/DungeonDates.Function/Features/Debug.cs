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
        var envList = new List<KeyValuePair<string?, string>>();

        foreach (System.Collections.DictionaryEntry entry in envVariables)
        {
            envList.Add(new(entry.Key.ToString(), entry.Value?.ToString() ?? ""));
        }
        
        foreach (var child in configuration.AsEnumerable())
        {
            envList.Add(new(child.Key, child.Value ?? ""));
        }

        return new OkObjectResult(envList);
    }
}