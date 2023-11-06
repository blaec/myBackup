using System;
using System.IO;
using System.Linq;
using log4net;
using Microsoft.Extensions.Configuration;

namespace myBackup.Utils
{
    public class FolderCleanup
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string _baseTarget;
        private static int _historySize;

        private FolderCleanup()
        {
        }
        
        public static FolderCleanup Init(IConfigurationSection configSection)
        {
            _baseTarget = configSection["target"];
            _historySize = Convert.ToInt32(configSection["history"]);

            return new FolderCleanup();
        }

        public void CleanupBackup()
        {
            DirectoryInfo[] dirs = new DirectoryInfo(_baseTarget).GetDirectories("*.*", SearchOption.TopDirectoryOnly);
            if (dirs.Length <= _historySize) return;
            
            DirectoryInfo[] oldDirs = dirs
                .OrderBy(dir => dir.CreationTime)
                .Take(dirs.Length - _historySize)
                .ToArray();

            DeleteDirectories(oldDirs);
        }
        
        private void DeleteDirectories(DirectoryInfo[] directories)
        {
            foreach (DirectoryInfo directory in directories)
            {
                Log.Info($"Deleting folder: {directory.FullName}");
                directory.Delete(true);
            }
        }
    }
}