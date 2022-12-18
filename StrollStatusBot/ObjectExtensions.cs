namespace StrollStatusBot;

internal static class ObjectExtensions
{
    public static long? ToLong(this object? o)
    {
        if (o is long l)
        {
            return l;
        }
        return long.TryParse(o?.ToString(), out l) ? l : null;
    }
}