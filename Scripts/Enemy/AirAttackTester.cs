using Godot;
using ProjectCleanSword.Scripts.Interfaces;

namespace ProjectCleanSword.Scripts.Enemy;

public partial class AirAttackTester : StaticBody2D, IDamageable
{
    [Export] public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    private float y;
    private double time;
    private float amplitude = 0.1f;

    public override void _Ready()
    {
        y = Position.Y;
    }

    public override void _PhysicsProcess(double delta)
    {
        time += delta;
        y += (float) Mathf.Sin(time) * amplitude; 
        Position = new Vector2(Position.X, y);
    }
}