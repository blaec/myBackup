using System;
using System.Threading.Tasks;
using log4net;
using myBackup.Jobs.Settings;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class CleanupJob : IJob
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static readonly JobKey DailyJobKey = new JobKey("dailyCleanupJob", JobGroup.Cleanup.ToString());
        public static readonly JobKey MonthlyJobKey = new JobKey("monthlyCleanupJob", JobGroup.Cleanup.ToString());

        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            
            try
            {
                string root = context.MergedJobDataMap.GetString("root");
            
                Log.Info($"{key} | {root} cleanup job started");

                FolderCleanup
                    .Init(ConfigUtils.Config.GetSection(root))
                    .CleanupBackup();
                
                await LogInfoAsync($"{key} | {root} cleanup job finished");
            }
            catch (Exception e)
            {
                await LogInfoAsync($"{key} | Failed to cleanup data: " + e.StackTrace);
            }
        }
        
        private static async Task LogInfoAsync(string message)
        {
            await Task.Run(() => Log.Info(message));
        }
    }
}