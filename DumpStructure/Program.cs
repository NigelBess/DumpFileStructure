using System.IO;
using System.Text;
using System.Windows;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var currentDirectory = Directory.GetCurrentDirectory();
        var depth = args.Length > 0 && int.TryParse(args[0], out var d) ? d : 3;
        var fileStructure = DirectoryStructureWriter.StructureString(currentDirectory, depth, true);

        Console.Write(fileStructure);
        Clipboard.SetText(fileStructure);
    }
}