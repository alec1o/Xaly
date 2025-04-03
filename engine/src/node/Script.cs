namespace XalyEngine;

public class Script : Node
{
    public string Fullname { get; set; } = string.Empty;
    internal Node Instance { get; set; } = null!;
    public Dictionary<string, string> Parameters { get; set; } = new();

    public Script()
    {
        Type = this.GetFullName();
    }

    public override void OnActivate() => Instance?.OnActivate();
    public override void OnInitialize() => Instance?.OnInitialize();
    public override void OnStart() => Instance?.OnStart();
    public override void OnFixedUpdate() => Instance?.OnFixedUpdate();
    public override void OnUpdate() => Instance?.OnUpdate();
    public override void OnRender() => Instance?.OnRender();
    public override void OnPostUpdate() => Instance?.OnPostUpdate();
    public override void OnDeactivate() => Instance?.OnDeactivate();
    public override void OnDestroy() => Instance?.OnDestroy();
}