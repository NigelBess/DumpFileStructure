
using System.IO;
using System.Text;

namespace DumpStructure;

internal class LeafFolder : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    public string ContentsSummary { get; }
    public LeafFolder(DirectoryInfo dir)
    {
        Name = dir.Name;

        // non-recursive counts: only direct children
        var filesHere = dir.EnumerateFiles().ToList();
        var dirsHereCount = dir.EnumerateDirectories().Count();

        // recursive size: this folder + all subfolders
        SizeBytes = dir
            .EnumerateFiles("*", SearchOption.AllDirectories)
            .Sum(f => f.Length);

        ContentsSummary = GenerateContentsSummary(filesHere.Count, dirsHereCount);
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

    public List<string> RenderAsLines() => new() { RenderAsLine() };
    public string RenderAsLine() => $"{Name} (Folder) - {SizeBytes.ToBytesString()} - {ContentsSummary}";
}
