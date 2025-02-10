using Microsoft.AspNetCore.Components;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient)
{
    private string? _content;
    
    public async Task CallApiAsync()
    {
        var response = await httpClient.GetAsync("/add");
        
        _content = await response.Content.ReadAsStringAsync();
    }
}