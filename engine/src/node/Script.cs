namespace XalyEngine;

public class Script : Node
{
    public string Assembly { get; set; } = string.Empty;
    public Dictionary<string, string> Parameters { get; set; } = new();

    public Script()
    {
        Type = this.GetFullName();
    }
}