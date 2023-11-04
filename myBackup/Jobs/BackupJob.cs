using System;
using System.Threading.Tasks;
using myBackup.Jobs.Settings;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class BackupJob : IJob
    {
        public static readonly JobKey DailyJobKey = new JobKey("dailyBackupJob", JobGroup.Backup.ToString());
        public static readonly JobKey MonthlyJobKey = new JobKey("monthlyBackupJob", JobGroup.Backup.ToString());
        
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            
            try
            {
                string root = context.MergedJobDataMap.GetString("root");
            
                Console.WriteLine($"{key} | {root} backup job started");

                FolderBackup folderBackup = FolderBackup.Init(root);
                folderBackup.CopyFolder();
                
                await Console.Out.WriteLineAsync($"{key} | {root} backup job finished");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"{key} | Failed to backup data: " + e.StackTrace);
            }
        }
    }
}