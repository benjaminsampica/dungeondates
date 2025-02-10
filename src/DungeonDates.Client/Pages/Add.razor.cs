using Microsoft.AspNetCore.Components;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient)
{
    private string? _content;
    private string? _uri;
    
    public async Task CallApiAsync()
    {
        var response = await httpClient.GetAsync("/add");

        _uri = response.RequestMessage?.RequestUri?.AbsoluteUri;
        _content = await response.Content.ReadAsStringAsync();
    }
}