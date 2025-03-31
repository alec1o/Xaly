namespace XalyEngine;

public class Scene
{
    public string Id { get; internal set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public IList<Entity> Entities { get; set; }
}