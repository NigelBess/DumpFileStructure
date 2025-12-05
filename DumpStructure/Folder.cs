namespace DumpStructure;

public record Folder
{
    public required string Name { get; init; }
    public required long SizeBytes { get; init; }
    public List<IFsObject> SubFolders { get; init; } = new();
    public List<File> Files { get; init; } = new();
    public string Render()
    {
        throw new NotImplementedException();
    }
}
