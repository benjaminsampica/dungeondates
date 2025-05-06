using DungeonDates.Shared.Features.Responses;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Respond(HttpClient httpClient, NavigationManager navigationManager, ISnackbar snackbar) : IDisposable
{
    [Parameter, EditorRequired] public Guid Id { get; set; }
    
    private GetDetailResponse? _response;
    private PostDetailRequest _request = new();
    private readonly CancellationTokenSource _cts = new();

    private string? _name;
    
    private bool _isLoading;
    
    protected override async Task OnInitializedAsync() => await LoadResponsesAsync();

    private async Task LoadResponsesAsync()
    {
        var response = await httpClient.GetAsync($"detail/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/StatusCode/404");
            return;
        }

        _response = await response.Content.ReadFromJsonAsync<GetDetailResponse>();
        _request.Id = Id;
    }

    private void OnProposedDateClick(GetDetailResponse.ProposedDate proposedDate)
    {
        var response = _request.ProposedDateResponses.FirstOrDefault(pdr => pdr.ProposedDateId == proposedDate.Id)
            ?? new PostDetailRequest.ProposedDate
            {
                ProposedDateId = proposedDate.Id
            };

        response.Accepted = !response.Accepted;
    }

    private async Task OnSaveClickedAsync()
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync($"detail/{Id}", _request, _cts.Token);

            response.EnsureSuccessStatusCode();
            
            snackbar.Add("Sucessfully recorded response.", Severity.Success);
        }
        catch
        {
            snackbar.Add("Something went wrong. Please try again later.", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            await LoadResponsesAsync();
        }
    }
    
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}