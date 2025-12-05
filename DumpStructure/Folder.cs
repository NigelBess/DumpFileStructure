using System.IO;

namespace DumpStructure;

public record Folder : IFsObject
{
    public string Name { get; }
    public long SizeBytes { get; }
    public IReadOnlyCollection<IFsObject> Contents { get; }
    public string Render()
    {
        throw new NotImplementedException();
    }

    public Folder(DirectoryInfo directory, int depth)
    {
        if (depth == 0) throw new InvalidOperationException($"{nameof(Folder)} requires a {nameof(depth)} of at least 1. For {nameof(depth)} == 0 use {nameof(LeafFolder)}");
        Name = directory.Name;
        SizeBytes = 0;

        var contents = new List<IFsObject>();

        // files directly in this directory
        foreach (var fileInfo in directory.EnumerateFiles())
        {
            var file = new File(fileInfo);
            contents.Add(file);
            SizeBytes += file.SizeBytes;
        }



        foreach (var subDir in directory.EnumerateDirectories())
        {
            IFsObject child = depth == 1
                ? new LeafFolder(subDir)          // summarize subtree at cutoff
                : new Folder(subDir, depth - 1);  // recurse deeper

            contents.Add(child);
            SizeBytes += child.SizeBytes;
        }

        Contents = contents;
    }
}
