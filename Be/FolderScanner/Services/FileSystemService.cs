using System.IO;
using System.IO.Abstractions;
using System.Security.Cryptography;
using FolderScanner.Interfaces;

namespace FolderScanner.Services;

public class FileSystemService : IFileSystemService
{
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<FileSystemService> _logger;

    public FileSystemService(
        IFileSystem fileSystem, 
        ILogger<FileSystemService> logger
        )
    {
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public IReadOnlyCollection<IFileSystemInfo> GetFileSystemInfos(string path)
    {
        _logger.LogInformation("Stared getting of FileSystemInfos for path {Path}", path);

        var directoryInfo = _fileSystem.DirectoryInfo.New(path);

        if (!directoryInfo.Exists)
        {
            throw new DirectoryNotFoundException("Entered directory does not exist");
        }

        var fileInfos = directoryInfo.GetFileSystemInfos("*", SearchOption.AllDirectories);

        _logger.LogInformation("Finished getting of FileSystemInfos for path {Path}", path);
        return fileInfos;
    }

    public string? CalculateMd5(IFileSystemInfo fileInfo)
    {
        if (fileInfo.Attributes == FileAttributes.Directory)
        {
            _logger.LogInformation("Skipping calculation of hash for fileInfo {FileInfo}", fileInfo);
            return null;
        }

        _logger.LogInformation("Calculating hash for fileInfo {FileInfo}", fileInfo);

        using var md5 = MD5.Create();
        using var stream = _fileSystem.File.OpenRead(fileInfo.FullName);
        var hash = md5.ComputeHash(stream);
        var stringHash = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        return stringHash;
    }
}