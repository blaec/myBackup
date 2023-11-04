using Quartz;

namespace myBackup.Jobs.Settings
{
    public class TriggerUtils
    {
        public static readonly CronScheduleBuilder DailySchedule = CronScheduleBuilder.DailyAtHourAndMinute(2,0);
        public static readonly CronScheduleBuilder MonthlySchedule = CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 3, 0);

        private TriggerUtils()
        {
        }
    }
}