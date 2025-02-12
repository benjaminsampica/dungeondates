using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace DungeonDates.Function.Features;

public class Health
{
    [Function("health")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous,  "get")] HttpRequest req) 
        => new OkObjectResult("Healthy");
}