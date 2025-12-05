
using DumpStructure;

namespace DumpStructureTests;

[TestClass]
public sealed class FsBuilderTests
{
    [TestMethod]
    public void TestBuild()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var folder = new Folder(directory, 4);
    }
}
