namespace FileStructureDump;

internal class Helpers
{
    public static string FolderSizeString(long sizeBytes, int countOfChildren)
    {
        if (sizeBytes == 0)
        {
            if (countOfChildren == 0) return "Empty";
            return "(Contains only empty subfolders)";
        }
        return sizeBytes.ToBytesString();
    }
}
