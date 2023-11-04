using System;
using System.Threading.Tasks;
using myBackup.Utils;
using Quartz;

namespace myBackup.Jobs
{
    public class BackupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobKey key = context.JobDetail.Key;
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string root = dataMap.GetString("root");
            
            Console.WriteLine($"{key} | {root} backup job started");
            try
            {
                FolderAction folderAction = FolderAction.Init(root);
                folderAction.CopyFolder();
                
                await Console.Out.WriteLineAsync($"{key} | {root} backup job finished");
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync($"{key} | Failed to backup data: " + e.StackTrace);
            }
        }
    }
}