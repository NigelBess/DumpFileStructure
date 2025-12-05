
using DumpStructure;

namespace DumpStructureTests;

[TestClass]
public sealed class FsBuilderTests
{
    [TestMethod]
    public void TestBuild()
    {
        var directory = Path.GetTempPath();
        var folder = FsBuilder.Build(directory);
    }
}
