using System;
using System.Threading.Tasks;
using log4net;
using myBackup.Jobs.Settings;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class BackupJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly JobKey DailyJobKey = new JobKey("dailyBackupJob", JobGroup.Backup.ToString());
        public static readonly JobKey MonthlyJobKey = new JobKey("monthlyBackupJob", JobGroup.Backup.ToString());
        public static readonly TriggerKey DailyTriggerKey = new TriggerKey("dailyBackupTrigger", JobGroup.Backup.ToString());
        public static readonly TriggerKey MonthlyTriggerKey = new TriggerKey("monthlyBackupTrigger", JobGroup.Backup.ToString());
        
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            
            try
            {
                string root = context.MergedJobDataMap.GetString("root");
            
                Log.Info($"{key} | {root} backup job started");

                FolderBackup
                    .Init(ConfigUtils.Config.GetSection(root))
                    .CopyFolder();
                
                await LogInfoAsync($"{key} | {root} backup job finished");
            }
            catch (Exception e)
            {
                await LogInfoAsync($"{key} | Failed to backup data: " + e.StackTrace);
            }
        }
        
        private static async Task LogInfoAsync(string message)
        {
            await Task.Run(() => Log.Info(message));
        }
    }
}