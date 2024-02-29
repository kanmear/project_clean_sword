using Godot;

namespace ProjectCleanSword.Scripts;

public partial class Main : Node
{
    public static Node2D Player { get; set; }

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause")) 
            GetTree().Quit();
    }
}