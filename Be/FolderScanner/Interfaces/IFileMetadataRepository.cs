using FolderScanner.Models;

namespace FolderScanner.Interfaces;

public interface IFileMetadataRepository
{
    string? GetFolderPath();
    void SetFolderPath(string? path);
    void SetMetadata(IEnumerable<FileMetadataModel> metadata);
    IDictionary<string, FileMetadataModel> GetMetadata();
    void AddFile(FileMetadataModel file);
    void RemoveFile(string fileName);
    void UpdateFile(FileMetadataModel file);
}