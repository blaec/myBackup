using System;
using System.Threading.Tasks;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class DailyBackupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                FolderAction folderAction = FolderAction.Init("daily");
                folderAction.CopyFolder();
                
                await Console.Out.WriteLineAsync("DailyBackupJob finished");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("DailyBackupJob | Failed to backup data: " + e.StackTrace);
            }
        }
    }
}