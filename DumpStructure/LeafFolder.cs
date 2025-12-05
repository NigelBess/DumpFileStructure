
namespace DumpStructure;

internal class LeafFolder : IFsObject
{
    public required string Name { get; init; }
    public required long SizeBytes { get; init; }
    public required string ContentsSummary { get; init; }
    public string Render()
    {
        throw new NotImplementedException();
    }
}
