using myBackup.model;

namespace myBackup
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            FolderAction folderAction = new FolderAction();
            folderAction.Init();
            folderAction.CopyFolder();
        }
    }
}