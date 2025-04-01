namespace XalyEngine;

public static class FullnameExtension
{
    public static string GetFullName<T>(this T _)
    {
        var type = typeof(T);
        return $"{type.Namespace}.{type.Name}";
    }
}