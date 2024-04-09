using Godot;
using ProjectCleanSword.Scripts.Interfaces;

namespace ProjectCleanSword.Scripts.Enemy;

public partial class AirAttackTester : StaticBody2D, IDamageable
{
    [Export] private Sprite2D sprite;
    [Export] private CollisionShape2D collisionShape;
    [Export] private Timer timerDisappear;
    [Export] private Timer timerReappear;
    
    private ShaderMaterial shaderMaterial;
    
    [Export] public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    private float y;
    private double time;
    private float amplitude = 0.1f;

    public override void _Ready()
    {
        y = Position.Y;
        
        shaderMaterial = sprite.Material.Duplicate() as ShaderMaterial; 
        sprite.Material = shaderMaterial;
    }

    public override void _PhysicsProcess(double delta)
    {
        time += delta;
        y += (float) Mathf.Sin(time) * amplitude; 
        Position = new Vector2(Position.X, y);
    }

    public void Damage(int amount)
    {
        shaderMaterial.SetShaderParameter("isWhite", true);
        
        timerDisappear.Start();
        timerReappear.Start();
    }

    private void OnTimerDisappearTimeout()
    {
        sprite.Visible = false;
        collisionShape.Disabled = true;
    }

    private void OnTimerReappearTimeout()
    {
        shaderMaterial.SetShaderParameter("isWhite", false);
        
        sprite.Visible = true;
        collisionShape.Disabled = false;
    }
}