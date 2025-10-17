namespace LearnKazakh.Bot.Core;

public static class ScheduleHelper
{
    public static bool IsNow(ScheduleModel scheduleModel)
    {
        DateTime now = DateTime.Now;

        return now.DayOfWeek == scheduleModel.DayOfWeek
               && now.Hour == scheduleModel.Hour
               && Math.Abs(now.Minute - scheduleModel.Minute) == 0;
    }
}

public class ScheduleModel
{
    public DayOfWeek DayOfWeek { get; set; }
    public int Hour { get; set; }
    public int Minute { get; set; }

    public DateTime? LastExecuted { get; set; }
}
