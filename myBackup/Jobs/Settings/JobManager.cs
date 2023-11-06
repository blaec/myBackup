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

            IJobDetail dailyCleanupJob = JobConstructorUtils.CreateJob<CleanupJob>("daily", CleanupJob.DailyJobKey);
            ITrigger dailyCleanupTrigger = JobConstructorUtils.CreateTrigger(CleanupJob.DailyTriggerKey, TriggerUtils.DailyCleanupSchedule);
            
            IJobDetail dailyBackupJob = JobConstructorUtils.CreateJob<BackupJob>("daily", BackupJob.DailyJobKey);
            ITrigger dailyBackupTrigger = JobConstructorUtils.CreateTrigger(BackupJob.DailyTriggerKey, TriggerUtils.DailyBackupSchedule);
            
            IJobDetail monthlyCleanupJob = JobConstructorUtils.CreateJob<CleanupJob>("monthly", CleanupJob.MonthlyJobKey);
            ITrigger monthlyCleanupTrigger = JobConstructorUtils.CreateTrigger(CleanupJob.MonthlyTriggerKey, TriggerUtils.MonthlyCleanupSchedule);

            IJobDetail monthlyBackupJob = JobConstructorUtils.CreateJob<BackupJob>("monthly", BackupJob.MonthlyJobKey);
            ITrigger monthlyBackupTrigger = JobConstructorUtils.CreateTrigger(BackupJob.MonthlyTriggerKey, TriggerUtils.MonthlyBackupSchedule);

            await scheduler.ScheduleJob(dailyCleanupJob, dailyCleanupTrigger);
            await scheduler.ScheduleJob(dailyBackupJob, dailyBackupTrigger);
            await scheduler.ScheduleJob(monthlyCleanupJob, monthlyCleanupTrigger);
            await scheduler.ScheduleJob(monthlyBackupJob, monthlyBackupTrigger);

            // will block until the last running job completes
            await builder.RunAsync();
        }
    }
}