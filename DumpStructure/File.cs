namespace DumpStructure;

public record File : IFsObject
{
    public required string Name { get; init; }
    public required long SizeBytes { get; init; }
    public string Render()
    {
        throw new NotImplementedException();
    }
}
