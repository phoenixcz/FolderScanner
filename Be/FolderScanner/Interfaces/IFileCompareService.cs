using System.IO.Abstractions;
using FolderScanner.Models;

namespace FolderScanner.Interfaces;

public interface IFileCompareService
{
    IReadOnlyCollection<ModifiedFileModel> GetModifiedFiles(IEnumerable<IFileSystemInfo> currentFileInfos,
        IDictionary<string, FileMetadataModel> storedFilesMetadata);
}