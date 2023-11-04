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

            // Console.ReadLine();
        }
    }
}