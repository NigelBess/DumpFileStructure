using System.IO;

namespace DumpStructure;

public record Folder : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    public IReadOnlyCollection<IFsObject> Contents { get; }

    private readonly LeafFolder _asLeafFolder;

    public Folder(DirectoryInfo directory, int depth)
    {
        if (depth == 0)
            throw new InvalidOperationException($"{nameof(Folder)} requires a {nameof(depth)} of at least 1. For {nameof(depth)} == 0 use {nameof(LeafFolder)}");

        _asLeafFolder = new(directory);

        Name = directory.Name;

        var contents = new List<IFsObject>();
        long size = 0;

        foreach (var fileInfo in directory.EnumerateFiles())
        {
            var file = new File(fileInfo);
            contents.Add(file);
            size += file.SizeBytes;
        }

        foreach (var subDir in directory.EnumerateDirectories())
        {
            IFsObject child = depth == 1
                ? new LeafFolder(subDir)          // summarize subtree at cutoff
                : new Folder(subDir, depth - 1);  // recurse deeper

            contents.Add(child);
            size += child.SizeBytes;
        }

        Contents = contents;
        SizeBytes = size;
    }

    public List<string> Render() => RenderAsEnumerable().ToList();
    }

    private IEnumerable<string> RenderAsEnumerable()
    {
        yield return _asLeafFolder.Render().First();
        var children = Contents.Select(c => c.Render()).ToList();
        foreach (var (idx, child) in children.Index())
        {
            var isLast = idx == children.Count - 1;
        }

    }
}
