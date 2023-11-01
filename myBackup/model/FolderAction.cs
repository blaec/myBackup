using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using myBackup.utils;

namespace myBackup.model
{
    public class FolderAction
    {
        private string _baseTarget;
        private string _target;
        private List<BackupLocation> _backupLocations;

        public FolderAction()
        {
        }

        public void Init()
        {
            _baseTarget = ConfigUtils.Config.GetSection("baseTarget").Value;
            _target = Path.Combine(_baseTarget, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            _backupLocations = ConfigUtils.Config.GetSection("backup").Get<List<BackupLocation>>();

            if (!Directory.Exists(_baseTarget))
            {
                Directory.CreateDirectory(_baseTarget);
            }
        }

        public void CopyFolder()
        {
            foreach (BackupLocation location in _backupLocations)
            {
                CopyFolder(new DirectoryInfo(location.path), "", location);
                foreach (DirectoryInfo di in new DirectoryInfo(location.path).GetDirectories("*.*", SearchOption.AllDirectories))
                {
                    CopyFolder(di, di.FullName.Replace(location.path, ""), location);
                }
               
            }
        }

        private void CopyFolder(DirectoryInfo di, string path, BackupLocation location)
        {
            Console.WriteLine($"[{di.GetDirectories().Length} - {di.GetFiles().Length}] {di.FullName}");
            string destinationFolderName = Path.Combine(_target, location.name + "\\" + path);
            if (!Directory.Exists(destinationFolderName))
            {
                Directory.CreateDirectory(destinationFolderName);
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                fi.CopyTo(Path.Combine(destinationFolderName, fi.Name), false);
            }
        }
    }
}