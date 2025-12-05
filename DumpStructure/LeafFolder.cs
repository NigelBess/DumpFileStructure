
using System.IO;
using System.Text;

namespace DumpStructure;

internal class LeafFolder : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    private readonly int _fileCount;
    private readonly int _subDirCount;
    public LeafFolder(DirectoryInfo dir)
    {
        Name = dir.Name;

        // non-recursive counts: only direct children
        _fileCount = dir.EnumerateFiles().ToList().Count;
        _subDirCount = dir.EnumerateDirectories().Count();

        // recursive size: this folder + all subfolders
        SizeBytes = dir
            .EnumerateFiles("*", SearchOption.AllDirectories)
            .Sum(f => f.Length);
    }

    private string GenerateContentsSummary()
    {
        var sb = new StringBuilder();
        var files = _fileCount;
        var directories = _subDirCount;
        if (files > 0)
        {
            sb.Append(files.ToCountString("file"));
            if (directories > 0) sb.Append(", ");
        }
        if (directories > 0) sb.Append(directories.ToCountString("subfolder"));
        return sb.ToString();
    }

    public List<string> RenderAsLines() => new() { Render() };

    private string Render()
    {
        var sb = new StringBuilder();
        sb.Append($"{Name} (Folder)");
        var totalItemCount = _fileCount + _subDirCount;
        if (totalItemCount > 0)
        {
            sb.Append($" - {GenerateContentsSummary()}");
        }
        sb.Append($" - {Helpers.FolderSizeString(SizeBytes, totalItemCount)}");
        return sb.ToString();
    }
}
