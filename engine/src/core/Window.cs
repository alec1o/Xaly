namespace XalyEngine;

public class Window
{
    public string Title { get; set; } = "XalyEngine";
    public int Width { get; set; } = 1280;
    public int Height { get; set; } = 720;
    public int FrameRate { get; set; } = -1;
    public bool IsFullscreen { get; set; } = false;
    public bool IsVisible { get; set; } = true;
    public bool IsInitialized { get; set; } = false;
}