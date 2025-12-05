namespace DumpStructure;

public static class Extensions
{
    public static string ToBytesString(this long value)
    {
        const long factor = 1024;
        const long kb = factor;
        const long mb = kb * factor;
        const long gb = mb * factor;

        if (value >= gb) return $"{value / gb}GB";
        if (value >= mb) return $"{value / mb}MB";
        if (value >= kb) return $"{value / kb}kB";
        return $"{value}B";
    }

    public static string ToCountString(this int count, string singular, string? plural = null)
        => $"{count} {(count == 1 ? singular : plural ?? singular + "s")}";
}
