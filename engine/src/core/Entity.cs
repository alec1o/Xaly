namespace XalyEngine;

public class Entity
{
    public string Id { get; internal set; } = Xid.New();
    public string Name { get; set; } = string.Empty;
    public Entity? Parent { get; set; }
    public IList<Node> Nodes { get; set; } = new List<Node>();
    public IList<Entity> Entities { get; set; } = new List<Entity>();
}