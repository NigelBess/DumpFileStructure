
using System.IO;
using System.Text;

namespace DumpStructure;

internal class LeafFolder : IFsObject
{
    public required string Name { get; init; }
    public required long SizeBytes { get; init; }
    public required string ContentsSummary { get; init; }
    public LeafFolder(DirectoryInfo dir)
    {
        Name = dir.Name;

        long size = 0;
        var files = 0;
        var dirs = 0;

        foreach (var _ in dir.EnumerateDirectories("*", SearchOption.AllDirectories)) dirs++;

        foreach (var file in dir.EnumerateFiles("*", SearchOption.AllDirectories))
        {
            files++;
            size += file.Length;
        }

        SizeBytes = size;
        ContentsSummary = GenerateContentsSummary(files, dirs);
    }

    private string GenerateContentsSummary(int files, int directories)
    {
        var sb = new StringBuilder();
        if (files > 0)
        {
            sb.Append(files.ToCountString("file"));
            if (directories > 0) sb.Append(", ");
        }
        if (directories > 0) sb.Append(directories.ToCountString("directory", "directories"));
        return sb.ToString();
    }

    public string Render() => $"{Name} - {SizeBytes.ToBytesString()} - {ContentsSummary}";
}
