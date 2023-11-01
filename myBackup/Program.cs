using System;
using myBackup.model;
using myBackup.utils;

namespace myBackup
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            ConfigUtils.Init();

            try
            {
                FolderAction folderAction = FolderAction.Init();
                folderAction.CopyFolder();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to backup data: " + e.StackTrace);
            }
        }
    }
}