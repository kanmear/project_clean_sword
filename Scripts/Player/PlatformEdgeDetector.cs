using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class PlatformEdgeDetector : RayCast2D
{
    [Export] private PlayerController playerController;

    public override void _Ready() => Enable(false);

    public override void _PhysicsProcess(double delta) => playerController.SetMovable(IsColliding());

    public void Enable(bool isEnabled) => SetPhysicsProcess(isEnabled);
}