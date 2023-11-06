using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace myBackup.Jobs.Settings
{
    public class JobManager
    {
        public static async Task Init()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((cxt, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();
                    });
                    services.AddQuartzHostedService(opt =>
                    {
                        opt.WaitForJobsToComplete = true;
                    });
                    services.Configure<LoggerFilterOptions>(options =>
                    {
                        options.AddFilter("Quartz", LogLevel.Error); // Adjust to your desired log level
                    });
                }).Build();
            
            var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            IJobDetail dailyCleanupJob = JobBuilder.Create<CleanupJob>()
                .WithIdentity(CleanupJob.DailyJobKey)
                .UsingJobData("root", "daily")
                .Build();
            ITrigger dailyCleanupTrigger = TriggerBuilder.Create()
                .WithIdentity("dailyCleanupTrigger", JobGroup.Cleanup.ToString())
                .WithSchedule(TriggerUtils.DailyCleanupSchedule)
                .Build();
            
            IJobDetail dailyBackupJob = JobBuilder.Create<BackupJob>()
                .WithIdentity(BackupJob.DailyJobKey)
                .UsingJobData("root", "daily")
                .Build();
            ITrigger dailyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("dailyBackupTrigger", JobGroup.Backup.ToString())
                .WithSchedule(TriggerUtils.DailyBackupSchedule)
                .Build();

            IJobDetail monthlyCleanupJob = JobBuilder.Create<CleanupJob>()
                .WithIdentity(CleanupJob.MonthlyJobKey)
                .UsingJobData("root", "monthly")
                .Build();
            ITrigger monthlyCleanupTrigger = TriggerBuilder.Create()
                .WithIdentity("monthlyCleanupTrigger", JobGroup.Cleanup.ToString())
                .WithSchedule(TriggerUtils.MonthlyCleanupSchedule)
                .Build();
            
            IJobDetail monthlyBackupJob = JobBuilder.Create<BackupJob>()
                .WithIdentity(BackupJob.MonthlyJobKey)
                .UsingJobData("root", "monthly")
                .Build();
            ITrigger monthlyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("monthlyBackupTrigger", JobGroup.Backup.ToString())
                .WithSchedule(TriggerUtils.MonthlyBackupSchedule)
                .Build();

            await scheduler.ScheduleJob(dailyCleanupJob, dailyCleanupTrigger);
            await scheduler.ScheduleJob(dailyBackupJob, dailyBackupTrigger);
            await scheduler.ScheduleJob(monthlyCleanupJob, monthlyCleanupTrigger);
            await scheduler.ScheduleJob(monthlyBackupJob, monthlyBackupTrigger);

            // will block until the last running job completes
            await builder.RunAsync();
        }
    }
}