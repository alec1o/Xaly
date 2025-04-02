using System.Reflection;

namespace XalyEngine;

public class Scene
{
    public string Id { get; internal set; } = Xid.New();
    public uint Index { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public IList<Entity> Entities { get; set; } = new List<Entity>();

    private bool _isStared = false;

    internal void Start()
    {
        if (_isStared) return;
        _isStared = true;

        foreach (var x in Entities)
        {
            x.Parent = null!;
            InitializeEntity(x);
        }

        void InitializeEntity(Entity entity)
        {
            entity.Id = Guid.NewGuid().ToString();

            foreach (var x in entity.Entities)
            {
                x.Parent = entity;
                InitializeEntity(x);
            }

            foreach (var x in entity.Nodes)
            {
                x.Id = Guid.NewGuid().ToString();
                x.Entity = entity;
                InitializeNode(x);
            }
        }

        void InitializeNode(Node node)
        {
            var type = node.Type;

            if (IsEqual<Script>(out var script))
            {
                var xType = Type.GetType(script.Fullname);
                ArgumentNullException.ThrowIfNull(xType);
                var xInstance = Activator.CreateInstance(xType);
                ArgumentNullException.ThrowIfNull(xInstance);
                var xNode = (Node)xInstance!;
                ArgumentNullException.ThrowIfNull(xNode);
                xNode.Entity = script.Entity;
                if (script.Parameters.Count > 0)
                {
                    var properties = xType.GetRuntimeProperties().Where(x => x.CanRead && x.CanWrite && x.DeclaringType!.Equals(xType)).ToArray();

                    foreach (var property in properties.ToArray())
                    {
                        Console.WriteLine(property.Name);
                        if (script.Parameters.TryGetValue(property.Name, out var value))
                        {
                            property.SetValue(xInstance, StringToObject(value, property.PropertyType));
                            continue;
                        }

                        script.Parameters[property.Name] = ObjectToString(property.GetValue(xInstance), property.PropertyType);
                    }
                }
                script.Instance = xNode;
            }

            bool IsEqual<T>(out T outname)
            {
                outname = default!;

                var success = type == FullnameExtension.GetFullName<T>(default!);
                if (success) outname = (T)(dynamic)node;
                return success;
            }
        }
    }

    private string ObjectToString(object? value, Type type)
    {
        Console.WriteLine(type.FullName);
        if (value == null) return string.Empty;
        var name = type.FullName;
        if (name == typeof(string).FullName) return (string)value;
        if (name == typeof(int).FullName) return ((int)value).ToString();
        if (name == typeof(float).FullName) return ((float)value).ToString();
        if (name == typeof(double).FullName) return ((double)value).ToString();
        throw new NotImplementedException(name);
    }

    private object? StringToObject(string value, Type type)
    {

        Console.WriteLine(type.FullName);
        if (value == null) return null;
        var name = type.FullName;
        if (name == typeof(string).FullName) return value;
        if (name == typeof(int).FullName) return int.Parse(value);
        if (name == typeof(float).FullName) return float.Parse(value);
        if (name == typeof(double).FullName) return double.Parse(value);
        throw new NotImplementedException(name);
    }

    internal void Stop()
    {
        foreach (var x in Entities.ToArray())
        {
            DestroyEntity(x);
            Entities.Remove(x);
        }

        void DestroyEntity(in Entity entity)
        {
            foreach (var x in entity.Entities.ToArray())
            {
                DestroyEntity(x);
            }

            foreach (var x in entity.Nodes.ToArray())
            {
                x.IsActive = false;
                x.OnDeactivate();
                x.OnDestroy();
            }

            foreach (var x in entity.Entities.ToArray())
            {
                entity.Entities.Remove(x);
            }
        }
    }

    internal void Update()
    {
        foreach (var x in Entities)
        {
            UpdateEntity(x);
        }

        void UpdateEntity(Entity entity)
        {
            foreach (var x in entity.Entities)
            {
                UpdateEntity(x);
            }

            foreach (var x in entity.Nodes)
            {
                if (!x.IsInitialized)
                {
                    x.IsInitialized = true;
                    x.OnInitialize();
                    x.OnStart();
                }
#if false
                if (Time.BeginFixed)
                {
                    x.OnFixedUpdate();
                    Time.EndFixed;
                }
#endif
                x.OnUpdate();
                x.OnPostUpdate();
            }
        }
    }
}