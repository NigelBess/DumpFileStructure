using System.IO;
using System.Text;

public static class DirectoryStructureWriter
{
    public static string GetStructure(string path, int maxDepth)
    {
        var sb = new StringBuilder();
        var trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        var rootName = Path.GetFileName(trimmed);
        if (string.IsNullOrEmpty(rootName)) rootName = trimmed;
        sb.AppendLine(rootName);
        Write(trimmed, sb, "", 1, maxDepth);
        return sb.ToString();
    }

    static void Write(string path, StringBuilder sb, string prefix, int depth, int maxDepth)
    {
        if (depth >= maxDepth) return;

        var dirs = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var total = dirs.Length + files.Length;

        for (var i = 0; i < dirs.Length; i++)
            WriteEntry(sb, dirs[i], prefix, depth, maxDepth, i == total - 1 && files.Length == 0, true);

        for (var i = 0; i < files.Length; i++)
            WriteEntry(sb, files[i], prefix, depth, maxDepth, dirs.Length + i == total - 1, false);
    }

    static void WriteEntry(StringBuilder sb, string path, string prefix, int depth, int maxDepth, bool isLast, bool isDir)
    {
        var name = Path.GetFileName(path);
        var branch = isLast ? "└── " : "├── ";
        var childPrefix = prefix + (isLast ? "    " : "│   ");

        sb.Append(prefix).Append(branch).Append(name);

        if (isDir && depth + 1 >= maxDepth)
        {
            Summarize(path, sb);
            sb.AppendLine();
            return;
        }

        sb.AppendLine();
        if (!isDir) return;

        Write(path, sb, childPrefix, depth + 1, maxDepth);
    }

    static void Summarize(string path, StringBuilder sb)
    {
        var dirs = 0;
        var files = 0;

        foreach (var _ in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories)) dirs++;
        foreach (var _ in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)) files++;

        sb.Append(" - ")
          .Append(files).Append(" files, ")
          .Append(dirs).Append(" subfolders");
    }
}
