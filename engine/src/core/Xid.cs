namespace XalyEngine;

public static class Xid
{
    public static string New()
    {
        return Guid.NewGuid().ToString();
    }
}