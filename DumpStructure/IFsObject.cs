namespace DumpStructure;

public interface IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    public List<string> Render();
}
