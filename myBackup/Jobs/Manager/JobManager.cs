using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace myBackup.Jobs.Manager
{
    public class JobManager
    {
        public static async Task Init()
        {
            // Grab the Scheduler instance from the Factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            IJobDetail dailyBackupJob = JobBuilder.Create<DailyBackupJob>()
                .WithIdentity("dailyBackupJob", "daily")
                .Build();
            IJobDetail monthlyBackupJob = JobBuilder.Create<MonthlyBackupJob>()
                .WithIdentity("monthlyBackupJob", "daily")
                .Build();

            ITrigger dailyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("dailyBackupTrigger", "monthly")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(2, 0))
                .Build();
            ITrigger monthlyBackupTrigger = TriggerBuilder.Create()
                .WithIdentity("monthlyBackupTrigger", "monthly")
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 3, 0))
                .Build();

            // Tell Quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(dailyBackupJob, dailyBackupTrigger);
            await scheduler.ScheduleJob(monthlyBackupJob, monthlyBackupTrigger);

            // some sleep to show what's happening
            await Task.Delay(TimeSpan.FromSeconds(60));
            
            // and last shut down the scheduler when you are ready to close your program
            await scheduler.Shutdown();
            
            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();
        }
    }
}