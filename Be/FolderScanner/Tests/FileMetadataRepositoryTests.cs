using FluentAssertions;
using FolderScanner.Models;
using FolderScanner.Services;
using Moq;
using Xunit;

namespace FolderScanner.Tests;

public class FileMetadataRepositoryTests
{
    private readonly FileMetadataRepository _subject;

    public FileMetadataRepositoryTests()
    {
        Mock<ILogger<FileMetadataRepository>> logger = new();
        _subject = new FileMetadataRepository(logger.Object);
    }

    [Theory]
    [InlineData("C:\\something")]
    [InlineData("")]
    [InlineData(null)]
    public void SetFolderPath_GetFolderPath_ReturnsCorrectValue(string path)
    {
        _subject.SetFolderPath(path);
        var result = _subject.GetFolderPath();

        result.Should().Be(path);
    }

    [Fact]
    public void SetMetadata_GetMetadata_ReturnsCorrectValue()
    {
        var metadata = new List<FileMetadataModel>
        {
            new() {FullName = "data1", Hash = "hash1", LastWriteTime = DateTime.UtcNow, Version = 3},
            new() {FullName = "data2", Hash = "hash2", LastWriteTime = DateTime.UtcNow.AddDays(-1), Version = 4},
        };

        _subject.SetMetadata(metadata);
        var result = _subject.GetMetadata();

        var expectedResult = metadata.ToDictionary(m => m.FullName);
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void AddFile_MetadataAlreadyExist_ThrowException()
    {
        var metadata = new FileMetadataModel {FullName = "name"};
        _subject.AddFile(metadata);

        _subject.Invoking(x => x.AddFile(metadata))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddFile_MetadataDontAlreadyExist_FileAdded()
    {
        var metadata = new FileMetadataModel {FullName = "name"};
        _subject.AddFile(metadata);

        var result = _subject.GetMetadata();

        var expectedResult = new Dictionary<string, FileMetadataModel>
        {
            {metadata.FullName, metadata}
        };
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void RemoveFile_MetadataDontExist_ThrowException()
    {
        _subject.Invoking(x => x.RemoveFile("metadata"))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void RemoveFile_MetadataExist_FileRemoved()
    {
        var metadata = new FileMetadataModel {FullName = "name"};
        _subject.AddFile(metadata);

        _subject.RemoveFile("name");

        var result = _subject.GetMetadata();
        result.Should().BeEmpty();
    }

    [Fact]
    public void UpdateFile_MetadataDontExist_ThrowException()
    {
        var metadata = new FileMetadataModel {FullName = "name"};
        _subject.Invoking(x => x.UpdateFile(metadata))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    public void UpdateFile_MetadataExist_FileUpdated()
    {
        var metadata = new FileMetadataModel
            {FullName = "name", Hash = "Hash1", LastWriteTime = DateTime.Now, Version = 1};
        _subject.AddFile(metadata);

        var updatedMetadata = new FileMetadataModel
            {FullName = "name", Hash = "Hash2", LastWriteTime = DateTime.Now.AddDays(1), Version = 2};
        _subject.UpdateFile(updatedMetadata);

        var result = _subject.GetMetadata();
        var expectedResult = new Dictionary<string, FileMetadataModel>
        {
            {updatedMetadata.FullName, updatedMetadata}
        };
        result.Should().BeEquivalentTo(expectedResult);
    }
}