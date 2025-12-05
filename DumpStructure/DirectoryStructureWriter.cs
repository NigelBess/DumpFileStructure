using System.IO;
using System.Text;

public static class DirectoryStructureWriter
{
    public static string StructureString(string path, int maxDepth, bool includeSizes)
    {
        var sb = new StringBuilder();
        var trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var rootName = Path.GetFileName(trimmed);
        if (string.IsNullOrEmpty(rootName)) rootName = trimmed;
        sb.AppendLine(rootName);
        Write(trimmed, sb, "", 1, maxDepth, includeSizes);
        return sb.ToString();
    }

    static void Write(string path, StringBuilder sb, string prefix, int depth, int maxDepth, bool includeSizes)
    {
        if (depth >= maxDepth) return;

        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var total = dirs.Length + files.Length;

        for (var i = 0; i < dirs.Length; i++)
            WriteEntry(sb, dirs[i], prefix, depth, maxDepth, i == total - 1 && files.Length == 0, true, includeSizes);

        for (var i = 0; i < files.Length; i++)
            WriteEntry(sb, files[i], prefix, depth, maxDepth, dirs.Length + i == total - 1, false, includeSizes);
    }

    static void WriteEntry(StringBuilder sb, string path, string prefix, int depth, int maxDepth, bool isLast, bool isDir, bool includeSizes)
    {
        var name = Path.GetFileName(path);
        var branch = isLast ? "└── " : "├── ";
        var childPrefix = prefix + (isLast ? "    " : "│   ");

        sb.Append(prefix).Append(branch).Append(name);

        if (!isDir)
        {
            if (includeSizes)
                sb.Append(" - ").Append(FormatSize(new FileInfo(path).Length));
            sb.AppendLine();
            return;
        }

        if (depth + 1 >= maxDepth)
        {
            Summarize(path, sb, includeSizes);
            sb.AppendLine();
            return;
        }

        sb.AppendLine();
        Write(path, sb, childPrefix, depth + 1, maxDepth, includeSizes);
    }

    static void Summarize(string path, StringBuilder sb, bool includeSizes)
    {
        var dirs = 0;
        var files = 0;
        long size = 0;

        foreach (var _ in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories)) dirs++;
        foreach (var f in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
        {
            files++;
            if (includeSizes) size += new FileInfo(f).Length;
        }

        if (files == 0 && dirs == 0) return;

        sb.Append(" - ");
        if (includeSizes && size > 0) sb.Append(FormatSize(size)).Append(" - ");
        if (files > 0) sb.Append($"{files} files");
        if (dirs > 0)
        {
            if (files > 0) sb.Append(", ");
            sb.Append($"{dirs} subfolders");
        }
    }

    static string FormatSize(long size)
    {
        const long kb = 1024;
        const long mb = kb * 1024;
        const long gb = mb * 1024;

        if (size >= gb) return $"{size / gb}GB";
        if (size >= mb) return $"{size / mb}MB";
        if (size >= kb) return $"{size / kb}kB";
        return $"{size}B";
    }
}
