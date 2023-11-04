using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace myBackup.Jobs.Manager
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
                }).Build();
            
            var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            var dailyBackupJob = JobBuilder.Create<BackupJob>()
                .WithIdentity("dailyBackupJob", "daily")
                .UsingJobData("root", "daily")
                .Build();
            IJobDetail monthlyBackupJob = JobBuilder.Create<BackupJob>()
                .WithIdentity("monthlyBackupJob", "monthly")
                .UsingJobData("root", "monthly")
                .Build();

            ITrigger dailyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("dailyBackupTrigger", "daily")
                // .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(2, 0))
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10,14))
                .Build();
            ITrigger monthlyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("monthlyBackupTrigger", "monthly")
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 3, 0))
                .Build();

            await scheduler.ScheduleJob(dailyBackupJob, dailyBackupTrigger);
            await scheduler.ScheduleJob(monthlyBackupJob, monthlyBackupTrigger);

            // will block until the last running job completes
            await builder.RunAsync();
        }
    }
}