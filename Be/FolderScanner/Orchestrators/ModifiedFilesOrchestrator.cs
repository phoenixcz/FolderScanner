using FolderScanner.Enums;
using FolderScanner.Interfaces;
using FolderScanner.Models;
using System.Collections.ObjectModel;
using System.IO.Abstractions;

namespace FolderScanner.Orchestrators;

public class ModifiedFilesOrchestrator : IModifiedFilesOrchestrator
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IFileMetadataRepository _fileMetadataRepository;
    private readonly IFileCompareService _fileCompareService;
    private readonly ILogger<ModifiedFilesOrchestrator> _logger;
    private readonly object _scanLocker = new();

    public ModifiedFilesOrchestrator(
        IFileSystemService fileSystemService, 
        IFileMetadataRepository fileMetadataRepository,
        IFileCompareService fileCompareService,
        ILogger<ModifiedFilesOrchestrator> logger
        )
    {
        _fileSystemService = fileSystemService;
        _fileMetadataRepository = fileMetadataRepository;
        _fileCompareService = fileCompareService;
        _logger = logger;
    }

    public IReadOnlyCollection<ModifiedFileModel> ScanFolder(string path)
    {
        lock (_scanLocker)
        {
            var currentFileInfos = _fileSystemService.GetFileSystemInfos(path);
            var storedMetadataPath = _fileMetadataRepository.GetFolderPath();

            if (path != storedMetadataPath)
            {
                _logger.LogInformation("Entered path {Path} is different than stored path {StoredMetadataPath}", path, storedMetadataPath);

                var newMetadata = TransformFileInfosToFileMetadata(currentFileInfos);
                _fileMetadataRepository.SetMetadata(newMetadata);
                _fileMetadataRepository.SetFolderPath(path);

                return new Collection<ModifiedFileModel>();
            }
            _logger.LogInformation("Entered path {Path} is same as stored path", path);

            var storedMetadata = _fileMetadataRepository.GetMetadata();
            var modifiedFiles = _fileCompareService.GetModifiedFiles(currentFileInfos, storedMetadata);

            UpdateModifiedFiles(modifiedFiles);

            return modifiedFiles;
        }
    }

    private IReadOnlyCollection<FileMetadataModel> TransformFileInfosToFileMetadata(IEnumerable<IFileSystemInfo> currentFileInfos)
    {
        var newMetadata = new Collection<FileMetadataModel>();

        _logger.LogInformation("Started transforming FileInfos to FileMetadata");

        foreach (var fileInfo in currentFileInfos)
        {
            _logger.LogDebug("Transforming FileInfo {FileInfo} into FileMetadata", fileInfo);

            var fileMetadata = new FileMetadataModel
            {
                FullName = fileInfo.FullName,
                LastWriteTime = fileInfo.LastWriteTime,
                Hash = _fileSystemService.CalculateMd5(fileInfo),
            };
            newMetadata.Add(fileMetadata);
        }

        _logger.LogInformation("Finished transforming FileInfos to FileMetadata");
        return newMetadata;
    }

    private void UpdateModifiedFiles(IEnumerable<ModifiedFileModel> modifiedFiles)
    {
        _logger.LogInformation("Started Updating modified files");

        foreach (var modifiedFile in modifiedFiles)
        {
            _logger.LogDebug("Updating modified file {ModifiedFile}", modifiedFile);
            switch (modifiedFile.Type)
            {
                case ModifiedFileType.Added:
                    _fileMetadataRepository.AddFile(modifiedFile);
                    break;

                case ModifiedFileType.Edited:
                    _fileMetadataRepository.UpdateFile(modifiedFile);
                    break;
                case ModifiedFileType.Deleted:
                    _fileMetadataRepository.RemoveFile(modifiedFile.FullName);
                    break;
                default:
                    throw new ArgumentException("Unknown modified file type", nameof(modifiedFile.Type));
            }
        }
        _logger.LogInformation("Finished Updating modified files");
    }
}