using System.IO;

namespace DumpStructure;

public record File : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }

    public File(FileInfo file)
    {
        Name = file.Name;
        SizeBytes = file.Length;
    }

    public string Render() => $"{Name} - {SizeBytes.ToBytesString()}";
}