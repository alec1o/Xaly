namespace XalyEngine;

public class Window
{
    public string Title { get; set; } = "XalyEngine";
    public int Width { get; set; } = 1280;
    public int Height { get; set; } = 720;
    public bool IsFullscreen { get; set; } = false;
    public bool IsVisible { get; set; } = false;
    public bool IsInitialized { get; set; } = false;
}