using DungeonDates.Function.Infrastructure.Databases;
using DungeonDates.Shared.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;

namespace DungeonDates.Function.Features;

public class Add(DungeonDatesDbContext dbContext)
{
    [Function("add")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous,  "post")] HttpRequest req)
    {
        var calendarDates = await JsonSerializer.DeserializeAsync<List<AddCalendarDate>>(req.Body, JsonSerializerOptions.Web) ?? [];

        if (calendarDates.Count == 0)
            return new BadRequestResult();

        var proposedDates = calendarDates.Select(cd => new ProposedDate
        {
            StartDate = cd.Start,
            EndDate = cd.End
        })
        .OrderBy(cd => cd.StartDate)
        .ToArray();

        var dungeonDate = new DungeonDate
        {
            Dates = proposedDates
        };
        
        dbContext.Add(dungeonDate);
        await dbContext.SaveChangesAsync(req.HttpContext.RequestAborted);
        
        return new OkObjectResult(new
        {
            dungeonDate.Id
        });
    }
}
