using DungeonDates.Shared;

namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient)
{
    private readonly IList<AddCalendarDate> _calendarDates = [];
    private RadzenScheduler<AddCalendarDate> _scheduler = null!;
    
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
        var newCalendarDate = new AddCalendarDate { Start = day, End = day.AddDays(1).AddSeconds(-1)};
        
        _calendarDates.Add(newCalendarDate);
        await _scheduler.Reload();
    }
    
    private async Task OnAppointmentSelectAsync(SchedulerAppointmentSelectEventArgs<AddCalendarDate> args)
    {
        _calendarDates.Remove(args.Data);
        await _scheduler.Reload();
    }
}