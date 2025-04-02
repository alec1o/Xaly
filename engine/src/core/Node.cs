namespace XalyEngine;

public abstract class Node
{
    public string Id { get; internal set; } = Xid.New();
    public Entity? Entity { get; internal set; }
    public string Type { get; internal set; } = string.Empty;
    public bool IsActive { get; set; }
    internal bool IsInitialized { get; set; }

    public virtual void OnInitialize() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnPostUpdate() { }
    public virtual void OnActivate() { }
    public virtual void OnDeactivate() { }
    public virtual void OnDestroy() { }
}