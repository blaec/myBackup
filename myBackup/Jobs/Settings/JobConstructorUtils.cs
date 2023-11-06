using Quartz;

namespace myBackup.Jobs.Settings
{
    public static class JobConstructorUtils
    {
        public static IJobDetail CreateJob<T>(string root, JobKey jobKey) where T : IJob
        {
            return JobBuilder.Create<T>()
                .WithIdentity(jobKey)
                .UsingJobData("root", root)
                .Build();
        }
        
        public static ITrigger CreateTrigger(TriggerKey triggerKey, CronScheduleBuilder schedule)
        {
            return TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .WithSchedule(schedule)
                .Build();
        }
    }
}