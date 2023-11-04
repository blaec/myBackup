using System;
using System.Threading.Tasks;
using myBackup.Jobs.Manager;
using myBackup.Objects;
using myBackup.Utils;

namespace myBackup
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigUtils.Init();
            
            await JobManager.Init();

            // try
            // {
            //     FolderAction folderAction = FolderAction.Init();
            //     folderAction.CopyFolder();
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine("Failed to backup data: " + e.StackTrace);
            // }

            Console.ReadLine();
        }
    }
}