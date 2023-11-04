﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using myBackup.Objects;

namespace myBackup.Utils
{
    public class FolderAction
    {
        private static string _baseTarget;
        private static string _target;
        private static List<BackupLocation> _backupLocations;

        private FolderAction()
        {
        }

        public static FolderAction Init(string group)
        {
            IConfigurationSection rootSection = ConfigUtils.Config.GetSection(group);
            _baseTarget = rootSection.GetSection("target").Value;
            _target = Path.Combine(_baseTarget, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            _backupLocations = rootSection.GetSection("source").Get<List<BackupLocation>>();

            if (!Directory.Exists(_baseTarget))
            {
                Directory.CreateDirectory(_baseTarget);
            }

            return new FolderAction();
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