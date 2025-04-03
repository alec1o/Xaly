using XalyEngine;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing;
using Silk.NET.Input;
using SilkWindow = Silk.NET.Windowing.Window;
using System.Drawing;

public static class Application
{
    public static bool IsActive { get; private set; }
    public static bool IsInitialized { get; private set; }
    public static Scene Scene { get; private set; } = new();
    public static XalyEngine.Window Window { get; private set; } = new();
    public static Input Input { get; private set; } = new();
    private static readonly object mutex = new();
    private static IWindow? _window;
    private static GL? _gl;

    internal static IList<Scene> Scenes = [];
    public static void Initialize(IList<Scene> scenes)
    {
        lock (mutex)
        {
            if (IsInitialized) return;

            if (scenes == null || scenes.Count <= 0)
                throw new Exception("No scene existent.");

            var indexes = new List<uint>();
            foreach (var scene in scenes)
            {
                if (indexes.Exists(x => x == scene.Index))
                    throw new Exception("Duplicated scene index.");
                indexes.Add(scene.Index);
            }

            if (!indexes.Exists(x => x == 0))
                throw new Exception("Main scene not found. Need to have Index=0");

            Scenes = scenes.DeepClone();
            IsInitialized = true;
        }
    }

    public static void Start()
    {
        lock (mutex)
        {
            if (!IsInitialized || IsActive) return;
            Scene = Scenes.First(x => x.Index == 0).DeepClone();
            IsActive = true;
        }

        var windowOptions = WindowOptions.Default;
        windowOptions.Size = new(Window.Width, Window.Height);
        windowOptions.Title = Window.Title;
        windowOptions.IsVisible = Window.IsVisible;
        windowOptions.FramesPerSecond = Window.FrameRate;
        windowOptions.WindowState = WindowState.Normal;

        _window = SilkWindow.Create(windowOptions);

        _window.Load += () =>
        {
            _gl = _window.CreateOpenGLES();
            _gl.ClearColor(Color.Aqua);
            Time.Start();
            Scene.Start();
        };

        _window.Update += (deltaTime) =>
        {
            Time.Delta = (float)deltaTime;
            Time.Update();
            Scene.Update();
        };

        _window.Render += (render) =>
        {
            _gl?.Clear(ClearBufferMask.ColorBufferBit);
            Scene.Render();
        };

        _window.Closing += () =>
        {
            Time.Stop();
            Scene.Stop();
        };

        _window.FramebufferResize += (size) =>
        {
            _gl?.Viewport(size);
        };

        _window.FocusChanged += (isFocus) =>
        {

        };

        _window.Run();
    }

    public static void Quit()
    {
        lock (mutex)
        {
            if (!IsInitialized) return;
            IsActive = false;
            IsInitialized = false;
            Scene = default!;
            Scenes = [];
        }

    }
}