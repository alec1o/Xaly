using XalyEngine;

public class PlayerMovement : Script
{
    public int velocity { get; set; }
    private float time;
    public string key { get; set; } = "Z";

    public override void OnInitialize()
    {
        Console.WriteLine("Player initialize");
    }

    public override void OnUpdate()
    {
        time += Time.Delta;
        Console.Write($"\rPlayer update: {time}");
    }

    public override void OnActivate()
    {
        Console.WriteLine("Player activate");
    }

    public override void OnDeactivate()
    {
        Console.WriteLine("Player deactivate");
    }

    public override void OnDestroy()
    {
        Console.WriteLine("Player destroy");
    }
    public override void OnStart()
    {
        Console.WriteLine($"Player Start, valocity: {velocity}, key: {key}");
    }
}