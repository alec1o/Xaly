namespace XalyEngine;

public abstract class Node
{
    public string Id { get; internal set; }
    public Entity Entity { get; internal set; }
    public bool IsActive { get; set; }
}