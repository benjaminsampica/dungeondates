using DungeonDates.Shared.Features.Responses;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Respond(HttpClient httpClient, NavigationManager navigationManager)
{
    [Parameter, EditorRequired] public Guid Id { get; set; }
    
    private GetDetailResponse? _response;
    
    private string? Name { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var response = await httpClient.GetAsync($"detail/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/StatusCode/404");
            return;
        }

        _response = await response.Content.ReadFromJsonAsync<GetDetailResponse>();
    }

    private async Task PostProposedDateResponse()
    {
        // need to think about how i want this to work.
    }
}