using System.Diagnostics;
using System.Text.Json;
using XalyEngine;

public static class Application
{
    public static bool IsActive { get; private set; }
    public static bool IsInitialized { get; private set; }
    public static Scene Scene { get; private set; } = new();
    public static Window Window { get; private set; } = new();
    public static Input Input { get; private set; } = new();
    private static readonly object mutex = new();

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

        Time.Start();
        Scene.Start();

        while (IsActive)
        {
            Time.Update();
            Scene.Update();

            //Console.Write($"\rGame loop ({Time.Seconds:0.00}s). [{Time.Delta:0.00000000}]. FPS: {Time.Frames}");
        }

        Scene.Stop();
        Time.Stop();
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