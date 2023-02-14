using FolderScanner.Enums;

namespace FolderScanner.Models;

public record ModifiedFileModel : FileMetadataModel
{
    public ModifiedFileType Type { get; set; }
}