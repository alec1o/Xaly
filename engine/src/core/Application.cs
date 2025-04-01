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

        foreach (var x in Scene.Entities)
        {
            x.Parent = null!;
            ProcessEntity(x);
        }

        var watch = new Stopwatch();
        var lastTime = watch.Elapsed.TotalSeconds;
        var deltaTime = 0d;
        var currentTime = 0d;
        var fps = 0;
        var fpsCounter = 0;
        var fpsTimer = 0f;
        watch.Start();

        while (IsActive)
        {
            currentTime = watch.Elapsed.TotalSeconds;
            deltaTime = (currentTime - lastTime) * Time.TimeScale;
            lastTime = currentTime;
            Time.PreciseDeltaTime = deltaTime;
            Time.TotalSeconds = currentTime;
            fpsTimer += Time.DeltaTime;
            fpsCounter++;
            if (fpsTimer >= 1f)
            {
                fps = fpsCounter;
                fpsCounter = 0;
                fpsTimer = 0;
            }
            Console.Write($"\rGame loop ({Time.TotalSeconds}s). [{Time.DeltaTime}]. FPS: {fps} - {fpsTimer}x{fpsCounter}");
            Thread.Sleep(1);
        }

        return;

        void ProcessEntity(Entity entity)
        {
            entity.Id = Guid.NewGuid().ToString();

            foreach (var x in entity.Entities)
            {
                x.Parent = entity;
                ProcessEntity(x);
            }

            foreach (var x in entity.Nodes)
            {
                x.Id = Guid.NewGuid().ToString();
                x.Entity = entity;
                ProcessNode(x);
            }
        }

        void ProcessNode(Node node)
        {
            var type = node.Type;

            if (IsEqual<Script>(out var script))
            {
                Console.WriteLine($"Starting Script: {script.Assembly}");
            }
            else if (IsEqual<Transform>(out var transform))
            {
                return;
            }

            return;

            bool IsEqual<T>(out T outname)
            {
                outname = default!;

                var success = type == FullnameExtension.GetFullName<T>(default!);

                if (success) outname = (T)(dynamic)node;
                return success;
            }
        }
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