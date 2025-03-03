using DungeonDates.Function.Infrastructure.Databases;
using DungeonDates.Shared.Features.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;

namespace DungeonDates.Function.Features.Responses;

public class Detail(DungeonDatesDbContext dbContext)
{
    [Function("detail")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "detail/{id}")] HttpRequest req)
    {
        var succeeded = Guid.TryParse(req.RouteValues[nameof(GetDetailRequest.Id)]?.ToString(), out var id);
        
        if(!succeeded) return new NotFoundResult();
        
        var dungeonDate = await dbContext.DungeonDates
            .Include(dd => dd.Dates)
            .ThenInclude(d => d.Responses)
            .AsNoTracking()
            .FirstOrDefaultAsync(dd => dd.Id == id);
        
        if (dungeonDate == null) return new NotFoundResult();
        
        var response = new GetDetailResponse
        {
            Id = dungeonDate.Id,
            ProposedDates = dungeonDate.Dates.Select(d => new GetDetailResponse.ProposedDate
            {
                Id = d.Id,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                Responses = d.Responses.Select(r => new GetDetailResponse.ProposedDate.ProposedDateResponse
                {
                    Name = r.Name,
                    Accepted = r.Accepted
                })
            })
        };
        
        return new OkObjectResult(response);
    }
}