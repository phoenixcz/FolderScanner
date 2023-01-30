using System.Collections.ObjectModel;

namespace FolderScanner.Models;

public class ScanFolderModel
{
    public IReadOnlyCollection<ModifiedFileModel> ModifiedFiles { get; set; } = new List<ModifiedFileModel>();
    public string[] Messages { get; set; } = Array.Empty<string>();
    public string Path { get; set; } = default!;
}