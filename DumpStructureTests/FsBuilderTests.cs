
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
            var folder = new Folder(root, depth: 2);
            var text = folder.Render();

            var expected = "Root - 18\n├──  root.txt - 4B\n├──  a - 4\n│    └──  a1.txt - 4B\n└──  b - 10\n     ├──  b1.txt - 4B\n     └──  nested (Folder) - 1 file - 6B";
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
