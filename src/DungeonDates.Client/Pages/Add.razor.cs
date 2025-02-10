using DungeonDates.Shared;
using System.Net.Http.Json;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient)
{
    private readonly IList<AddCalendarDate> _calendarDates = [];
    private RadzenScheduler<AddCalendarDate> _scheduler = null!;

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
        if (_calendarDates.Count == 0)
        {
            _invalidForm = true;
            return;
        }

        // TODO: Snackbar + show link to copy.
        await httpClient.PostAsJsonAsync("add", _calendarDates);
    }
}