using System.IO;

namespace DumpStructure;

internal record File : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }

    public File(FileInfo file)
    {
        Name = file.Name;
        SizeBytes = file.Length;
    }

    public List<string> RenderAsLines() => new() { $"{Name} - {SizeBytes.ToBytesString()}" };
}