using Quartz;

namespace myBackup.Jobs.Settings
{
    public class TriggerUtils
    {
        public static readonly CronScheduleBuilder DailyCleanupSchedule = CronScheduleBuilder.DailyAtHourAndMinute(1,30);
        public static readonly CronScheduleBuilder DailyBackupSchedule = CronScheduleBuilder.DailyAtHourAndMinute(2,0);

        public static readonly CronScheduleBuilder MonthlyCleanupSchedule = CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 2, 30);
        public static readonly CronScheduleBuilder MonthlyBackupSchedule = CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 3, 30);

        private TriggerUtils()
        {
        }
    }
}