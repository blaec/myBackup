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
            
            FolderAction folderAction = new FolderAction();
            folderAction.Init();
            folderAction.CopyFolder();
        }
    }
}