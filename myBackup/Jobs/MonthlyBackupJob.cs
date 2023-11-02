using System;
using System.Threading.Tasks;
using myBackup.Objects;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class MonthlyBackupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                FolderAction folderAction = FolderAction.Init("monthly");
                folderAction.CopyFolder();
                
                await Console.Out.WriteLineAsync("MonthlyBackupJob finished");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync("MonthlyBackupJob | Failed to backup data: " + e.StackTrace);
            }
        }
    }
}