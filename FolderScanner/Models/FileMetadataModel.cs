namespace FolderScanner.Models;

public record FileMetadataModel
{
    public string FullName { get; set; } = default!;
    public DateTime LastWriteTime { get; set; }
    public string? Hash { get; set; }
    public int Version { get; set; } = 1;
}