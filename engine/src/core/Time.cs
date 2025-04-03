using System.Diagnostics;

namespace XalyEngine;

public static class Time
{
    public static float Delta { get; internal set; }
    public static float Scale { get; set; } = 1;
    public static float Seconds { get; private set; }
    public static int FPS { get; private set; }

    private static bool _isInitialized = false;
    private static float _fpsTimer;
    private static int _fpsCounter;

    internal static void Start()
    {
        if (_isInitialized) return;

        Seconds = 0;
        Delta = 0;
        FPS = 0;
        Scale = 1;

        _isInitialized = true;
    }

    internal static void Stop()
    {
        if (!_isInitialized) return;

        FPS = 0;
        Delta = 0;

        _isInitialized = false;
    }

    internal static void Update()
    {
        if (!_isInitialized) return;

        Seconds += Delta;
        _fpsTimer += Delta;
        _fpsCounter++;

        if (_fpsTimer >= 1F)
        {
            FPS = _fpsCounter;
            _fpsCounter = 0;
            _fpsTimer = 0;
        }
    }
}