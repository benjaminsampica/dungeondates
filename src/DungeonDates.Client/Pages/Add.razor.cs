using DungeonDates.Shared.Features;
using Heron.MudCalendar;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient, ISnackbar snackbar, IDialogService dialogService) : IDisposable
{
    private readonly IList<CalendarItem> _calendarDates = [];
    private MudCalendar _scheduler = null!;
    private readonly CancellationTokenSource _cts = new();
    private bool _isLoading;
    
    private void OnCellClicked(DateTime date)
    {
        var existingCalendarDate = _calendarDates.FirstOrDefault(c => c.Start == date);
        var userMeantToRemove = existingCalendarDate is not null;
        if (userMeantToRemove)
        {
            _calendarDates.Remove(existingCalendarDate!);
            _scheduler.Refresh();

            return;
        }
        
        var newCalendarDate = new CalendarItem { Start = date, End = date.AddDays(1).AddSeconds(-1), AllDay = true };
        
        _calendarDates.Add(newCalendarDate);
        _scheduler.Refresh();
    }
    
    private void OnItemClicked(CalendarItem item)
    {
        _calendarDates.Remove(item);
        _scheduler.Refresh();
    }

    private async Task OnSaveClickedAsync()
    {
        _isLoading = true;
        
        if (_calendarDates.Count == 0)
        {
            _isLoading = false;
            snackbar.Add("You must add at least one date.", Severity.Error);
            return;
        }

        try
        {
            var response = await httpClient.PostAsJsonAsync("add", _calendarDates.Select(cd => new AddCalendarDate { Start = cd.Start, End = cd.End!.Value }));

            response.EnsureSuccessStatusCode();

            var successfulAddResponse = await response.Content.ReadFromJsonAsync<SuccessfulAddResponse>();
            
            await dialogService.ShowAsync<AddSuccess>(null, new DialogParameters { { nameof(SuccessfulAddResponse.Id), successfulAddResponse!.Id } }, new() { BackdropClick = false, CloseButton = true});
        }
        catch
        {
            snackbar.Add("Something went wrong. Please try again later.", Severity.Error);
        }
        finally
        {
            _isLoading = false;
            _calendarDates.Clear();
            _scheduler.Refresh();
        }
    }
    
    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}

public record SuccessfulAddResponse
{
    public Guid Id { get; set; }
}