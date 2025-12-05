
using DumpStructure;
using File = System.IO.File;

namespace DumpStructureTests;

[TestClass]
public sealed class FsBuilderTests
{
    [TestMethod]
    public void TestBuild()
    {
        WithTestFolder(root =>
        {
            var folder = new Folder(root, depth: 4);
            var text = folder.Render();

            var expected = "Root (Folder) - 18B - 1 file, 2 directories\r\n├──  root.txt - 4B\r\n├──  a (Folder) - 4B - 1 file\r\n│    └──  a1.txt - 4B\r\n└──  b (Folder) - 10B - 1 file, 1 directory\r\n     ├──  b1.txt - 4B\r\n     └──  nested (Folder) - 6B - 1 file\r\n          └──  n1.txt - 6B";
            Assert.AreEqual(expected, text);
        });
    }

    static void WithTestFolder(Action<DirectoryInfo> testBody)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), "Root");
        var root = Directory.CreateDirectory(rootPath);

        try
        {
            // simple mock tree:
            // root/
            //   root.txt
            //   a/
            //     a1.txt
            //   b/
            //     b1.txt
            //     nested/
            //       n1.txt

            File.WriteAllText(Path.Combine(root.FullName, "root.txt"), "root");

            var a = root.CreateSubdirectory("a");
            File.WriteAllText(Path.Combine(a.FullName, "a1.txt"), "aaaa");

            var b = root.CreateSubdirectory("b");
            File.WriteAllText(Path.Combine(b.FullName, "b1.txt"), "bbbb");

            var nested = b.CreateSubdirectory("nested");
            File.WriteAllText(Path.Combine(nested.FullName, "n1.txt"), "nested");

            testBody(root);
        }
        finally
        {
            // clean up recursively
            root.Refresh();
            if (root.Exists)
                root.Delete(recursive: true);
        }
    }
}
