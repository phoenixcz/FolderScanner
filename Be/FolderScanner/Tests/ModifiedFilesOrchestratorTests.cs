using System.Collections.ObjectModel;
using System.IO.Abstractions;
using FolderScanner.Interfaces;
using FolderScanner.Orchestrators;
using Moq;
using Xunit;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FolderScanner.Enums;
using FolderScanner.Models;

namespace FolderScanner.Tests;

public class ModifiedFilesOrchestratorTests
{
    private readonly Mock<IFileSystemService> _fileSystemService;
    private readonly Mock<IFileMetadataRepository> _fileMetadataRepository;
    private readonly Mock<IFileCompareService> _fileCompareService;
    private readonly ModifiedFilesOrchestrator _subject;
    private readonly Mock<ILogger<ModifiedFilesOrchestrator>> _logger;

    public ModifiedFilesOrchestratorTests()
    {
        _fileSystemService = new Mock<IFileSystemService>();
        _fileMetadataRepository = new Mock<IFileMetadataRepository>();
        _fileCompareService = new Mock<IFileCompareService>();
        _logger = new Mock<ILogger<ModifiedFilesOrchestrator>>();

        _subject = new ModifiedFilesOrchestrator(_fileSystemService.Object, _fileMetadataRepository.Object,
            _fileCompareService.Object, _logger.Object);
    }

    [Fact]
    public void ScanFolder_NewPath_StoresPathInRepository()
    {
        const string path = "newPath";
        _fileSystemService.Setup(f => f.GetFileSystemInfos(It.IsAny<string>())).Returns(new List<IFileSystemInfo>());

        _subject.ScanFolder(path);

        _fileMetadataRepository.Verify(r => r.SetFolderPath(It.Is<string>(s => s == path)));
    }

    [Fact]
    public void ScanFolder_NewPath_StoresMetadata()
    {
        // Arrange
        const string path = "newPath";
        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"C:\currentFileInfoName", new MockFileData("Some content")},
        });

        var currentFileInfos = new List<IFileSystemInfo>
        {
            new MockFileInfo(fileSystem, @"C:\currentFileInfoName")
        };

        _fileSystemService.Setup(f => f.GetFileSystemInfos(It.IsAny<string>())).Returns(currentFileInfos);

        // Act
        _subject.ScanFolder(path);

        // Assert
        _fileMetadataRepository.Verify(r => r.SetMetadata(It.Is<Collection<FileMetadataModel>>(s =>
            s[0].FullName == currentFileInfos[0].FullName &&
            s[0].LastWriteTime == currentFileInfos[0].LastWriteTime &&
            s[0].Version == 1
        )));
    }

    [Fact]
    public void ScanFolder_NewPath_ReturnsCorrectData()
    {
        const string path = "newPath";
        _fileSystemService.Setup(f => f.GetFileSystemInfos(It.IsAny<string>())).Returns(new List<IFileSystemInfo>());

        var result = _subject.ScanFolder(path);

        result.Should().BeEmpty();
    }

    [Fact]
    public void ScanFolder_SamePath_UpdatesMetadata()
    {
        // Arrange
        var addedFile = new ModifiedFileModel { FullName = "addedFile", Type = ModifiedFileType.Added };
        var modifiedFile = new ModifiedFileModel { FullName = "modifiedFile", Type = ModifiedFileType.Edited };
        var deletedFile = new ModifiedFileModel { FullName = "deletedFile", Type = ModifiedFileType.Deleted };

        var modifiedFiles = new List<ModifiedFileModel>
        {
            addedFile,
            modifiedFile,
            deletedFile,
        };

        var path = MockServicesForSamePath(modifiedFiles);

        // Act
        _subject.ScanFolder(path);

        // Assert
        _fileMetadataRepository.Verify(r => r.AddFile(It.Is<ModifiedFileModel>(f => f == addedFile)));
        _fileMetadataRepository.Verify(r => r.UpdateFile(It.Is<ModifiedFileModel>(f => f == modifiedFile)));
        _fileMetadataRepository.Verify(r => r.RemoveFile(It.Is<string>(f => f == deletedFile.FullName)));
    }

    [Fact]
    public void ScanFolder_SamePath_ReturnsCorrectData()
    {
        // Arrange
        var addedFile = new ModifiedFileModel { FullName = "addedFile", Type = ModifiedFileType.Added };
        var modifiedFile = new ModifiedFileModel { FullName = "modifiedFile", Type = ModifiedFileType.Edited };
        var deletedFile = new ModifiedFileModel { FullName = "deletedFile", Type = ModifiedFileType.Deleted };

        var modifiedFiles = new List<ModifiedFileModel>
        {
            addedFile,
            modifiedFile,
            deletedFile,
        };

        var path = MockServicesForSamePath(modifiedFiles);

        // Act
        var result = _subject.ScanFolder(path);

        // Assert
        result.Should().BeEquivalentTo(modifiedFiles);
    }

    private string MockServicesForSamePath(IReadOnlyCollection<ModifiedFileModel> modifiedFiles)
    {
        const string path = "path";

        _fileSystemService.Setup(f => f.GetFileSystemInfos(It.IsAny<string>())).Returns(new List<IFileSystemInfo>());
        _fileMetadataRepository.Setup(r => r.GetFolderPath()).Returns(path);
        _fileMetadataRepository.Setup(r => r.GetMetadata()).Returns(new Dictionary<string, FileMetadataModel>());

        _fileCompareService.Setup(c => c.GetModifiedFiles(It.IsAny<IReadOnlyCollection<IFileSystemInfo>>(),
            It.IsAny<IDictionary<string, FileMetadataModel>>())).Returns(modifiedFiles);
        return path;
    }
}