using DungeonDates.Shared.Features;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient, NotificationService notificationService, DialogService dialogService)
{
    private readonly IList<AddCalendarDate> _calendarDates = [];
    private RadzenScheduler<AddCalendarDate> _scheduler = null!;
    private bool _isLoading;

    private bool _invalidForm;
    
    private void OnSlotRender(SchedulerSlotRenderEventArgs args)
    {
        if (args.Start.Date == DateTime.Today)
        {
            args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
        }
    }
    
    private async Task OnSlotSelectAsync(SchedulerSlotSelectEventArgs args)
    {
        var day = args.Start.Date;
        
        var existingCalendarDate = _calendarDates.FirstOrDefault(c => c.Start == day);
        var userMeantToRemove = existingCalendarDate is not null;
        if (userMeantToRemove)
        {
            _calendarDates.Remove(existingCalendarDate!);
            await _scheduler.Reload();

            return;
        }
        
        var newCalendarDate = new AddCalendarDate { Start = day, End = day.AddDays(1).AddSeconds(-1)};
        
        _calendarDates.Add(newCalendarDate);
        await _scheduler.Reload();
    }
    
    private async Task OnAppointmentSelectAsync(SchedulerAppointmentSelectEventArgs<AddCalendarDate> args)
    {
        _calendarDates.Remove(args.Data);
        await _scheduler.Reload();
    }

    private async Task OnSaveAsync()
    {
        _isLoading = true;
        
        if (_calendarDates.Count == 0)
        {
            _invalidForm = true;
            _isLoading = false;
            return;
        }

        _invalidForm = false;

        try
        {
            var response = await httpClient.PostAsJsonAsync("add", _calendarDates);

            response.EnsureSuccessStatusCode();

            var successfulAddResponse = await response.Content.ReadFromJsonAsync<SuccessfulAddResponse>();
            
            await dialogService.OpenAsync<AddSuccess>("Successfully Created Dungeon Date",
                new() { { nameof(SuccessfulAddResponse.Id), successfulAddResponse!.Id } },
                new()
                {
                    Style = "border: 1px solid var(--rz-success);"
                });
        }
        catch
        {
            notificationService.Notify(new() { Summary = "Error", Detail = "Something went wrong. Please try again later.", Severity = NotificationSeverity.Error });
        }
        finally
        {
            _isLoading = false;
            _calendarDates.Clear();
            await _scheduler.Reload();
        }
    }
}

public record SuccessfulAddResponse
{
    public Guid Id { get; set; }
}