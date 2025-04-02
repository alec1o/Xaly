using XalyEngine;

public class PlayerMovement : Script
{
    public int velocity { get; set; }
    private float time, _timer;
    public string key { get; set; } = "Z";
    public string name { get; set; } = "Player";

    public override void OnInitialize()
    {
        Console.WriteLine($"Player ({name}) initialize");
    }

    public override void OnUpdate()
    {
        time += Time.Delta;
        _timer += Time.Delta;
        if (_timer >= 5)
        {
            _timer = 0;
            Console.WriteLine($"\rPlayer ({name}) update: {time}");
        }
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