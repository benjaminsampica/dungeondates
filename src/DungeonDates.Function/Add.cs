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
