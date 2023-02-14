using FolderScanner.Enums;

namespace FolderScanner.DTOs;

public class ModifiedFileDto
{
    public string FullName { get; set; } = default!;
    public int Version { get; set; } = 1;
    public ModifiedFileType Type { get; set; }
}