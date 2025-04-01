namespace XalyEngine;

public class Scene
{
    public string Id { get; internal set; } = Xid.New();
    public uint Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public IList<Entity> Entities { get; set; } = new List<Entity>();
}