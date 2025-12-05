using FileStructureDump;
using System.IO;
using System.Text;
using System.Windows;

static class Program
{
    const int defaultDepth = 3;

    [STAThread]
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var currentDirectory = Directory.GetCurrentDirectory();
        var depth = args.Length > 0 && int.TryParse(args[0], out var d) ? d : defaultDepth;
        var folder = new Folder(new(currentDirectory), depth);
        var text = folder.Render();
        Console.WriteLine(text);
        Clipboard.SetText(text);
    }
}