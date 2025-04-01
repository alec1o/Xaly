
using XalyEngine;

var scene = new Scene
{
    Name = "Game",
};

var entity = new Entity
{
    Name = "Player",
    Nodes = new List<Node>()
};

entity.Nodes.Add(new Transform());
entity.Nodes.Add(new Script()
{
    Assembly = "XalyProject.PlayerMovement",
    Parameters = new()
    {
        { "velocity", "100" },
        { "key", "A" }
    },
});

scene.Entities.Add(entity);

Application.Initialize([scene]);
Application.Start();
Application.Quit();