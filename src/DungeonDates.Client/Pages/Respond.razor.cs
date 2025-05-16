using Blazored.LocalStorage;
using DungeonDates.Shared.Features.Responses;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Respond(HttpClient httpClient, NavigationManager navigationManager, ISnackbar snackbar, ILocalStorageService localStorageService) : IDisposable
{
    [Parameter, EditorRequired] public Guid Id { get; set; }
    
    private GetDetailResponse? _response;
    private readonly PostDetailRequest _request = new();
    private readonly CancellationTokenSource _cts = new();
    private RespondLocalStorageState? _localStorageState = new();
    private string? _name;
    
    private bool _isLoading;
    private bool _hasAlreadyResponded;
    
    protected override async Task OnInitializedAsync() => await LoadPageStateAsync();

    private async Task LoadPageStateAsync()
    {
        var response = await httpClient.GetAsync($"detail/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            navigationManager.NavigateTo("/StatusCode/404");
            return;
        }
        
        _request.Id = Id;
        _response = await response.Content.ReadFromJsonAsync<GetDetailResponse>();
        _localStorageState = await localStorageService.GetItemAsync<RespondLocalStorageState>(Id.ToString(), _cts.Token);

        if (_localStorageState != null)
        {
            _name = _localStorageState.Name;
            _hasAlreadyResponded = true;
        }

        _request.ProposedDateResponses.Clear();
        foreach (var proposedDate in _response!.ProposedDates)
        {
            var proposedDateResponse = new PostDetailRequest.ProposedDate
            {
                ProposedDateId = proposedDate.Id, 
                Accepted = _hasAlreadyResponded && (proposedDate.Responses.FirstOrDefault(r => r.Name == _name)?.Accepted ?? false)
            };
            
            _request.ProposedDateResponses.Add(proposedDateResponse);
        }
    }

    private async Task OnSaveClickedAsync()
    {
        _isLoading = true;

        if (string.IsNullOrEmpty(_name))
        {
            _isLoading = false;
            snackbar.Add("You must add a name.", Severity.Error);
            return;
        }
        
        try
        {
            _request.Name = _name;   
            _request.HasAlreadyResponded = _hasAlreadyResponded;
            var response = await httpClient.PostAsJsonAsync($"detail/{Id}", _request, _cts.Token);

            response.EnsureSuccessStatusCode();
            
            await localStorageService.SetItemAsync(_request.Id.ToString(), new { Name = _name }, CancellationToken.None);
            
            snackbar.Add("Successfully saved your response.", Severity.Success);
        }
        catch
        {
            snackbar.Add("Something went wrong. Please try again later.", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            await LoadPageStateAsync();
        }
    }
    
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    private class RespondLocalStorageState
    {
        public string? Name { get; set; } = string.Empty;
    }
}