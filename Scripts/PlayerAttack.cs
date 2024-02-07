namespace ProjectCleanSword.Scripts;

using Godot;
using Interfaces;

public partial class PlayerAttack : Area2D
{
    private void OnBodyEntered(Node2D body)
    {
        if (body is IDamageable damageable) 
            damageable.Damage(10);
    }
}