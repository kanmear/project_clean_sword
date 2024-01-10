namespace ProjectCleanSword.Scripts.Interfaces;

using Godot;

public interface IMovable
{
    float MovingSpeed { get; set; }
    bool IsFacingRight { get; set; }
    
    void Move(Vector2 velocity) { }
}