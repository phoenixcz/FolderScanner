using FolderScanner.Interfaces;
using FolderScanner.Models;
using System.IO;

namespace FolderScanner.Services;

public class FileMetadataRepository : IFileMetadataRepository
{
    private readonly ILogger<FileMetadataRepository> _logger;
    private string? _folderPath;
    private Dictionary<string, FileMetadataModel> _metadata = new();

    public FileMetadataRepository(ILogger<FileMetadataRepository> logger)
    {
        _logger = logger;
    }
    
    public string? GetFolderPath()
    {
        return _folderPath;
    }

    public void SetFolderPath(string? path)
    {
        _logger.LogDebug("Setting FolderPath to {FolderPath}", path);
        _folderPath = path;
    }

    public void SetMetadata(IEnumerable<FileMetadataModel> metadata)
    {
        _logger.LogDebug("Setting Metadata to {FileMetadata}", metadata);
        _metadata = metadata.ToDictionary(m => m.FullName);
    }

    public IDictionary<string, FileMetadataModel> GetMetadata()
    {
        return _metadata;
    }

    public void AddFile(FileMetadataModel file)
    {
        _logger.LogDebug("Adding file Metadata {FileMetadata}", file);

        CheckMetadataDontExist(file.FullName);

        _metadata.Add(file.FullName, file);
    }

    public void RemoveFile(string fullName)
    {
        _logger.LogDebug("Removing file Metadata for file {FullName}", fullName);

        CheckMetadataExist(fullName);

        RemoveMetadata(fullName);
    }

    public void UpdateFile(FileMetadataModel file)
    {
        _logger.LogDebug("Updating file Metadata {FileMetadata}", file);

        CheckMetadataExist(file.FullName);
        RemoveMetadata(file.FullName);

        _metadata.Add(file.FullName, file);
    }

    private void CheckMetadataExist(string fullName)
    {
        if (!_metadata.ContainsKey(fullName))
        {
            throw new ArgumentException(
                $"Metadata for file {fullName} were not found");
        }
    }

    private void CheckMetadataDontExist(string fullName)
    {
        if (_metadata.ContainsKey(fullName))
        {
            throw new ArgumentException($"Metadata for file {fullName} were found when not supposed to exist");
        }
    }

    private void RemoveMetadata(string fullName)
    {
        var success = _metadata.Remove(fullName);

        if (!success)
        {
            throw new InvalidOperationException($"Could not remove metadata for file {fullName}");
        }
    }
}