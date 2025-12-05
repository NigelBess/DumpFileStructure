
using System.IO;

namespace DumpStructure;

public static class FsBuilder
{
    public static Folder BuildFolder(string path, int maxDepth)
        => (Folder)Build(new DirectoryInfo(path), 0, maxDepth);

    static IFsObject Build(DirectoryInfo dir, int depth, int maxDepth)
    {
        if (depth >= maxDepth)
        {
            var (size, summary) = Summarize(dir);
            return new LeafFolder()
        }

        var contents = new List<IFsObject>();
        long size = 0;

        foreach (var subDir in dir.EnumerateDirectories())
        {
            var child = Build(subDir, depth + 1, maxDepth);
            contents.Add(child);
            size += child.SizeBytes;
        }

        foreach (var fi in dir.EnumerateFiles())
        {
            var file = new File
            {
                Name = fi.Name,
                SizeBytes = fi.Length
            };
            contents.Add(file);
            size += file.SizeBytes;
        }

        return new Folder
        {
            Name = dir.Name,
            SizeBytes = size,
            Contents = contents
        };
    }
}
