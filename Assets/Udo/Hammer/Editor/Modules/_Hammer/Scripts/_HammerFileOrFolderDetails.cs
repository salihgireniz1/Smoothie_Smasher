namespace Udo.Hammer.Editor.Modules._Hammer
{
    // ReSharper disable once InconsistentNaming
    public class _HammerFileOrFolderDetails
    {
        public string[] Folders;
        public string FileOrFolder;
        public bool IsFile;

        public _HammerFileOrFolderDetails(string[] folders, string fileOrFolder, bool ısFile)
        {
            Folders = folders;
            FileOrFolder = fileOrFolder;
            IsFile = ısFile;
        }
    }
}