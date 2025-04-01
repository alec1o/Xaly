using System.Reflection;

namespace XalyEngine;

internal static class CloneExtension
{
    public static T DeepClone<T>(this T obj)
    {
        if (obj == null) return default!;

        var type = obj.GetType();
        var copy = Activator.CreateInstance(type);

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        {
            field.SetValue(copy, field.GetValue(obj) is ICloneable cloneable ? cloneable.Clone() : field.GetValue(obj));
        }

        return (T)copy!;
    }
}