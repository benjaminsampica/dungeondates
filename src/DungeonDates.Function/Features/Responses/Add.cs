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
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "detail/{id}")] HttpRequest req)
    {
        var succeeded = Guid.TryParse(req.RouteValues[nameof(PostDetailRequest.Id)]?.ToString(), out var id);
        
        if(!succeeded) return new NotFoundResult();
        
        var dungeonDate = await dbContext.DungeonDates
            .FirstOrDefaultAsync(d => d.Id == id.ToString(), req.HttpContext.RequestAborted);
        
        if (dungeonDate == null) return new NotFoundResult();
        
        var request = await JsonSerializer.DeserializeAsync<PostDetailRequest>(req.Body, JsonSerializerOptions.Web, req.HttpContext.RequestAborted);

        foreach (var proposedDate in request!.ProposedDateResponses)
        {
            var proposedDateResponse = new ProposedDateResponse
            {
                Accepted = proposedDate.Accepted,
                Name = request.Name
            };
            
            var existingProposedDate = dungeonDate.Dates.First(d => d.Id == proposedDate.ProposedDateId);
            existingProposedDate.Responses.Add(proposedDateResponse);
        }
        
        await dbContext.SaveChangesAsync(req.HttpContext.RequestAborted);
        
        return new OkResult();
    }
}