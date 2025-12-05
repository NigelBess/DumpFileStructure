namespace FileStructureDump;

public record Folder : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    public IReadOnlyCollection<IFsObject> Contents { get; }

    public Folder(DirectoryInfo directory, int depth)
    {
        if (depth == 0)
            throw new InvalidOperationException($"{nameof(Folder)} requires a {nameof(depth)} of at least 1. For {nameof(depth)} == 0 use {nameof(LeafFolder)}");

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

    public List<string> RenderAsLines() => RenderAsEnumerable().ToList();
    public string Render() => string.Join("\n", RenderAsLines());
    private IEnumerable<string> RenderAsEnumerable()
    {
        yield return $"{Name} - {Helpers.FolderSizeString(SizeBytes, Contents.Count)}";
        var children = Contents.Select(c => c.RenderAsLines()).ToList();
        foreach (var (outerIdx, child) in children.Index())
        {
            var isLast = outerIdx == children.Count - 1;
            var firstLineStart = isLast ? "└── " : "├── ";
            var nonFirstLineStart = isLast ? "    " : "│   ";
            foreach (var (innerIdx, childSubStr) in child.Index())
            {
                var lineStart =
                    innerIdx == 0 ? firstLineStart : nonFirstLineStart;
                yield return $"{lineStart} {childSubStr}";
            }
        }

    }
}
