using FolderScanner.Models;

namespace FolderScanner.Interfaces;

public interface IModifiedFilesOrchestrator
{
    IReadOnlyCollection<ModifiedFileModel> ScanFolder(string path);
}