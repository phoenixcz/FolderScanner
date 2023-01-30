using System.IO.Abstractions;

namespace FolderScanner.Interfaces;

public interface IFileSystemService
{
    IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(string path);
    string? CalculateMd5(IFileSystemInfo fileInfo);
}