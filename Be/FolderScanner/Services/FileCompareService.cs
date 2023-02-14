using System.Collections.ObjectModel;
using System.IO.Abstractions;
using FolderScanner.Enums;
using FolderScanner.Interfaces;
using FolderScanner.Models;

namespace FolderScanner.Services;

public class FileCompareService : IFileCompareService
{
    private readonly IFileSystemService _fileSystemService;
    private readonly ILogger<FileCompareService> _logger;

    public FileCompareService(IFileSystemService fileSystemService,
        ILogger<FileCompareService> logger)
    {
        _fileSystemService = fileSystemService;
        _logger = logger;
    }

    public IReadOnlyCollection<ModifiedFileModel> GetModifiedFiles(IEnumerable<IFileSystemInfo> currentFileInfos,
        IDictionary<string, FileMetadataModel> storedFilesMetadata)
    {
        _logger.LogInformation("Getting of modified files stared");

        var modifiedFiles = new Collection<ModifiedFileModel>();
        var unprocessedMetadata = new Dictionary<string, FileMetadataModel>(storedFilesMetadata);

        foreach (var currentFileInfo in currentFileInfos)
        {
            _logger.LogDebug("Checking file {FileInfo} for modifications", currentFileInfo);
            // File was edited
            if (unprocessedMetadata.TryGetValue(currentFileInfo.FullName, out var storedFileMetadata)
                && currentFileInfo.LastWriteTime > storedFileMetadata.LastWriteTime)
            {
                var currentHash = _fileSystemService.CalculateMd5(currentFileInfo);
                if (currentHash != storedFileMetadata.Hash)
                {
                    AddEditedFileToCollection(modifiedFiles, currentFileInfo, currentHash, storedFileMetadata);
                }
            }

            // File was added
            if (storedFileMetadata == null)
            {
                AddAddedFileToCollection(modifiedFiles, currentFileInfo);
                continue;
            }

            unprocessedMetadata.Remove(storedFileMetadata.FullName);
        }

        // Rest of unprocessed files didn't have match in current files, thus they were deleted
        foreach (var deletedFile in unprocessedMetadata)
        {
            AddDeletedFileToCollection(modifiedFiles, deletedFile.Key);
        }

        _logger.LogInformation("Getting of modified files ended");

        return modifiedFiles;
    }

    private void AddDeletedFileToCollection(
        ICollection<ModifiedFileModel> modifiedFiles, 
        string fullName
        )
    {
        var modifiedFile = new ModifiedFileModel
        {
            FullName = fullName,
            Type = ModifiedFileType.Deleted
        };
        modifiedFiles.Add(modifiedFile);
        _logger.LogDebug("Modified file {modifiedFile} added as type deleted", modifiedFile);
    }

    private void AddAddedFileToCollection(
        ICollection<ModifiedFileModel> modifiedFiles, 
        IFileSystemInfo currentFileInfo
        )
    {
        var modifiedFile = new ModifiedFileModel
        {
            FullName = currentFileInfo.FullName,
            Type = ModifiedFileType.Added,
            LastWriteTime = currentFileInfo.LastWriteTime,
            Hash = _fileSystemService.CalculateMd5(currentFileInfo)
        };
        modifiedFiles.Add(modifiedFile);

        _logger.LogDebug("Modified file {modifiedFile} added as type added", modifiedFile);
    }

    private void AddEditedFileToCollection(
        ICollection<ModifiedFileModel> modifiedFiles,
        IFileSystemInfo currentFileInfo, 
        string? currentHash,
        FileMetadataModel storedFileMetadata
        )
    {
        var modifiedFile = new ModifiedFileModel
        {
            FullName = currentFileInfo.FullName,
            Type = ModifiedFileType.Edited,
            LastWriteTime = currentFileInfo.LastWriteTime,
            Hash = currentHash,
            Version = storedFileMetadata.Version + 1
        };

        modifiedFiles.Add(modifiedFile);

        _logger.LogDebug("Modified file {modifiedFile} added as type modified", modifiedFile);
    }
}