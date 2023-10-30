using System;
using System.IO;

namespace myBackup.model
{
    public class FolderAction
    {
        private string _baseFolder;
        private string _target;
        private string _source;
        private string _sourceName;

        public FolderAction()
        {
        }

        public void Init()
        {
            _baseFolder = @"C:\Users\blaec\Downloads\_backup";
            _target = Path.Combine(_baseFolder, DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            _source = @"C:\Users\blaec\Pictures\My Albums\2007";
            _sourceName = "my_albums";
            
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }
        }

        public void CopyFolder()
        {
            CopyFolder(new DirectoryInfo(_source), "");
            foreach (DirectoryInfo di in new DirectoryInfo(_source).GetDirectories("*.*", SearchOption.AllDirectories))
            {
                CopyFolder(di, di.FullName.Replace(_source, ""));
            }
        }

        private void CopyFolder(DirectoryInfo di, string path)
        {
            Console.WriteLine($"[{di.GetDirectories().Length} - {di.GetFiles().Length}] {di.FullName}");
            string destinationFolderName = Path.Combine(_target, _sourceName + "\\" + path);
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