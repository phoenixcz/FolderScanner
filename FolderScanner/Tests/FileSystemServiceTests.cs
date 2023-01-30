using System.IO.Abstractions.TestingHelpers;
using FluentAssertions;
using FolderScanner.Services;
using Moq;
using Xunit;

namespace FolderScanner.Tests;

public class FileSystemServiceTests
{
    private const string MockedFile1 = @"c:\something\myfile.txt";
    private const string MockedFile2 = @"c:\something\myfile2.txt";
    private readonly FileSystemService _subject;
    private MockFileSystem _fileSystem;

    public FileSystemServiceTests()
    {
        _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            {MockedFile1, new MockFileData("Some content")},
            {MockedFile2, new MockFileData("Some other content")},
        });

        var logger = new Mock<ILogger<FileSystemService>>();

        _subject = new FileSystemService(_fileSystem, logger.Object);
    }

    [Fact]
    public void GetFileSystemInfos_DirectoryNotExist_ThrowsException()
    {
        _subject.Invoking(x => x.GetFileSystemInfos(@"c:\nonExistingFolder"))
            .Should().Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void GetFileSystemInfos_ValidDirectory_ReturnsFileSystemInfos()
    {
        var result = _subject.GetFileSystemInfos(@"c:\something");

        result.Should()
            .HaveCount(2)
            .And.Contain(x => x.FullName == MockedFile1)
            .And.Contain(x => x.FullName == MockedFile2);
    }

    [Fact]
    public void CalculateMd5_NonExistingFilename_ThrowsException()
    {
        var fileInfo = new MockFileInfo(_fileSystem, @"C:\nonExistingFileInfoName");

        _subject.Invoking(x => x.CalculateMd5(fileInfo))
            .Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void CalculateMd5_ExistingFile_ReturnsHash()
    {
        var fileInfo = new MockFileInfo(_fileSystem, MockedFile1);

        var result = _subject.CalculateMd5(fileInfo);

        result.Should().NotBeNullOrEmpty();
    }
}