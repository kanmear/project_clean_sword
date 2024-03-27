namespace ProjectCleanSword.Scripts.Player;

using Godot;
using Interfaces;

public partial class PlayerAttack : Area2D
{
    private static void OnBodyEntered(Node2D body)
    {
        if (body is IDamageable damageable) 
            damageable.Damage(10);
    }
}