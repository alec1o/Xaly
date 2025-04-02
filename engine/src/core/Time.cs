using System.Diagnostics;

namespace XalyEngine;

public static class Time
{
    public static float Delta => (float)_delta;
    public static float Scale { get => (float)_scale; set => _scale = value; }
    public static float Seconds => (float)_currentTime;
    public static int Frames => (int)_currentFps;

    private static bool _isInitialized = false;
    private static double _lastTime, _currentTime, _fpsTimer, _delta, _scale = 1, _currentFps;
    private static int _fpsCounter;
    private static Stopwatch _watch = new();

    internal static void Start()
    {
        if (_isInitialized) return;
        _watch.Restart();
        _isInitialized = true;
    }

    internal static void Stop()
    {
        if (!_isInitialized) return;
        _watch.Stop();
        _isInitialized = false;
    }

    internal static void Update()
    {
        if (!_isInitialized) return;

        _currentTime = _watch.Elapsed.TotalSeconds;
        _delta = (_currentTime - _lastTime) * _scale;
        _lastTime = _currentTime;
        _fpsCounter++;
        _fpsTimer += _delta;

        if (_fpsTimer >= 1F)
        {
            _currentFps = _fpsCounter;
            _fpsCounter = 0;
            _fpsTimer = 0;
        }
    }
}