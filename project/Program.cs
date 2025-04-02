
using System.Reflection;
using XalyEngine;

var scene = new Scene
{
    Name = "Game",
};

var entity = new Entity
{
    Name = "Player"
};

entity.Nodes.Add(new Transform());
entity.Nodes.Add(new Script()
{
    Fullname = "PlayerMovement, XalyProject",
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