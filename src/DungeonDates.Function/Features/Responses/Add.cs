using DungeonDates.Function.Infrastructure.Databases;
using DungeonDates.Shared.Features.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DungeonDates.Function.Features.Responses;

public class Add(DungeonDatesDbContext dbContext)
{
    [Function("add-detail")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "detail/{proposedDateId}")] HttpRequest req)
    {
        var succeeded = Guid.TryParse(req.RouteValues[nameof(PostDetailRequest.ProposedDateId)]?.ToString(), out var id);
        
        if(!succeeded) return new NotFoundResult();
        
        var proposedDate = await dbContext.ProposedDates
            .FirstOrDefaultAsync(d => d.Id == id, req.HttpContext.RequestAborted);
        
        if (proposedDate == null) return new NotFoundResult();
        
        var request = await JsonSerializer.DeserializeAsync<PostDetailRequest>(req.Body);
        
        var proposedDateResponse = new ProposedDateResponse
        {
            Name = request!.Name,
            Accepted = request.Accepted
        };
        
        proposedDate.Responses.Add(proposedDateResponse);
        await dbContext.SaveChangesAsync(req.HttpContext.RequestAborted);
        
        return new OkObjectResult(null);
    }
}