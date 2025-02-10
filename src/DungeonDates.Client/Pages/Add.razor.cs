using DungeonDates.Shared;


namespace DungeonDates.Client.Pages;

public partial class Add(HttpClient httpClient)
{
    private RadzenScheduler<AddCalendarDate> _scheduler = null!;

    private IList<AddCalendarDate> _calendarDates = [];
    
    private void OnSlotRender(SchedulerSlotRenderEventArgs args)
    {
        // Highlight today
        if (args.Start.Date == DateTime.Today)
        {
            args.Attributes["style"] = "background: var(--rz-scheduler-highlight-background-color, rgba(255,220,40,.2));";
        }
    }
    
    private async Task OnSlotSelectAsync(SchedulerSlotSelectEventArgs args)
    {
        var newCalendarDate = new AddCalendarDate { Start = args.Start, End = args.Start.AddDays(1).AddSeconds(-1)};
        
        _calendarDates.Add(newCalendarDate);
        await _scheduler.Reload();
    }
    
    private async Task OnAppointmentSelectAsync(SchedulerAppointmentSelectEventArgs<AddCalendarDate> args)
    {
        _calendarDates.Remove(args.Data);
        await _scheduler.Reload();
    }
    
    public async Task CallApiAsync()
    {
        var response = await httpClient.GetAsync("add");
    }
}