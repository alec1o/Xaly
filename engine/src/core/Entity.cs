namespace XalyEngine;

public class Entity
{
    public string Id { get; internal set; }
    public string Name { get; set; }
    public Entity Parent { get; set; }
    public IList<Node> Nodes { get; set; }
    public IList<Entity> Entities { get; set; }
}