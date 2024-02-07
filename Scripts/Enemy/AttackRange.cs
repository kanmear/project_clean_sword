namespace ProjectCleanSword.Scripts.Enemy;

using Godot;

public partial class AttackRange : Area2D
{
    [Export] private Enemy enemy;
    
    private void OnBodyEntered(Node2D body)
    {
        enemy.AttackPlayer();
    }
}