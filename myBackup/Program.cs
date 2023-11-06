using System.Threading.Tasks;
using log4net;
using myBackup.Jobs.Settings;
using myBackup.Utils;


namespace myBackup
{
    internal class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static async Task Main(string[] args)
        {
            Log.Info("Starting...");
            
            ConfigUtils.Init();
            FolderCleanup.Init(ConfigUtils.Config.GetSection("daily")).CleanupBackup();
            FolderCleanup.Init(ConfigUtils.Config.GetSection("monthly")).CleanupBackup();
            // await JobManager.Init();

            // Console.ReadLine();
        }
    }
}