using System.Collections.ObjectModel;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FolderScanner.Enums;
using FolderScanner.Interfaces;
using FolderScanner.Models;
using FolderScanner.Services;
using Moq;
using Xunit;

namespace FolderScanner.Tests;

public class FileCompareServiceTests
{
    private readonly FileCompareService _subject;
    private readonly Mock<IFileSystemService> _fileSystemService;

    private const string CurrentFileInfoHash = "currentFileInfoHash";

    public FileCompareServiceTests()
    {
        _fileSystemService = new Mock<IFileSystemService>();
        Mock<ILogger<FileCompareService>> logger = new();
        _subject = new FileCompareService(_fileSystemService.Object, logger.Object);
    }

    [Fact]
    public void GetModifiedFiles_NewFile_ReturnsAddedFile()
    {
        // Arrange
        var currentFileInfos = MockCurrentFileInfos();

        var storedFilesMetadata = new Dictionary<string, FileMetadataModel>();

        // Act
        var result = _subject.GetModifiedFiles(currentFileInfos, storedFilesMetadata);

        // Assert
        var expectedResult = new List<ModifiedFileModel>
        {
            new()
            {
                FullName = currentFileInfos[0].FullName,
                Type = ModifiedFileType.Added,
                LastWriteTime = currentFileInfos[0].LastWriteTime,
                Hash = CurrentFileInfoHash
            }
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetModifiedFiles_FileRemoved_ReturnsDeletedFile()
    {
        // Arrange
        const string storedMetadataFileName = @"c:\myfile.txt";

        var currentFileInfos = new List<IFileSystemInfo>();

        var storedFilesMetadata = new Dictionary<string, FileMetadataModel>
        {
            {storedMetadataFileName, new FileMetadataModel()}
        };

        // Act
        var result = _subject.GetModifiedFiles(currentFileInfos, storedFilesMetadata);

        // Assert
        var expectedResult = new List<ModifiedFileModel>
        {
            new()
            {
                FullName = storedMetadataFileName,
                Type = ModifiedFileType.Deleted,
            }
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetModifiedFiles_FileEdited_ReturnsModifiedFile()
    {
        // Arrange
        const int storedMetadataVersion = 5;

        var currentFileInfos = MockCurrentFileInfos();

        var storedFilesMetadata = new Dictionary<string, FileMetadataModel>
        {
            {
                currentFileInfos[0].FullName, new FileMetadataModel
                {
                    FullName = currentFileInfos[0].FullName,
                    LastWriteTime = currentFileInfos[0].LastWriteTime.AddDays(-1),
                    Hash = "storedMetadataHash",
                    Version = storedMetadataVersion
                }
            }
        };

        // Act
        var result = _subject.GetModifiedFiles(currentFileInfos, storedFilesMetadata);

        // Assert
        var expectedResult = new List<ModifiedFileModel>
        {
            new()
            {
                FullName = currentFileInfos[0].FullName,
                Type = ModifiedFileType.Modified,
                Hash = CurrentFileInfoHash,
                LastWriteTime = currentFileInfos[0].LastWriteTime,
                Version = storedMetadataVersion + 1
            }
        };

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void GetModifiedFiles_NoChanges_ReturnsEmptyModifiedFiles()
    {
        // Arrange
        var currentFileInfos = MockCurrentFileInfos();

        var storedFilesMetadata = new Dictionary<string, FileMetadataModel>
        {
            {
                currentFileInfos[0].FullName, new FileMetadataModel
                {
                    FullName = currentFileInfos[0].FullName,
                    LastWriteTime = currentFileInfos[0].LastWriteTime,
                    Hash = CurrentFileInfoHash,
                    Version = 1
                }
            }
        };

        // Act
        var result = _subject.GetModifiedFiles(currentFileInfos, storedFilesMetadata);

        // Assert

        result.Should().BeEmpty();
    }


    private List<IFileSystemInfo> MockCurrentFileInfos()
    {
        _fileSystemService.Setup(f => f.CalculateMd5(It.IsAny<IFileSystemInfo>()))
            .Returns(CurrentFileInfoHash);

        var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {@"C:\currentFileInfoName", new MockFileData("Some content")},
        });

        var currentFileInfos = new List<IFileSystemInfo>
        {
            new MockFileInfo(fileSystem, @"C:\currentFileInfoName")
        };
        return currentFileInfos;
    }
}