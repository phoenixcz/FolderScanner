using FolderScanner.Models;

namespace FolderScanner.Interfaces;

public interface IScanFolderOrchestrator
{
    ScanFolderModel ScanFolder(string path);
}