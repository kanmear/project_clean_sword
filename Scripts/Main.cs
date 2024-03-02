using Godot;

namespace ProjectCleanSword.Scripts;

public partial class Main : Node
{
    public static Node2D Player { get; set; }
    [Export] private PackedScene enemy;
    private RandomNumberGenerator generator;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Hidden;
        generator = new RandomNumberGenerator();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("pause")) 
            GetTree().Quit();

        if (Input.IsActionJustPressed("spawn_grunt_DEBUG"))
        {
            Enemy.Enemy enemyInst = enemy.Instantiate() as Enemy.Enemy;
            if (generator.RandiRange(0, 1) == 1)
                enemyInst.Position = new Vector2(-200f, 504f);
            else
                enemyInst.Position = new Vector2(1300f, 504f);
            GetParent().AddChild(enemyInst);
        }
    }
}