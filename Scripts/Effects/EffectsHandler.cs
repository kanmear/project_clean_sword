using Godot;

namespace ProjectCleanSword.Scripts.Effects;

public static class EffectsHandler
{
    public static readonly PackedScene PlayerJumpVfx = GD.Load<PackedScene>(ResourceLocations.PlayerJumpVfx);
    public static readonly PackedScene PlayerLandVfx = GD.Load<PackedScene>(ResourceLocations.PlayerLandVfx);
    public static readonly PackedScene PlayerDashVfx = GD.Load<PackedScene>(ResourceLocations.PlayerDashVfx);
    public static readonly PackedScene PlayerWallkickVfx = GD.Load<PackedScene>(ResourceLocations.PlayerWallkickVfx);

    private static readonly Vector2 FlippedH = new(-1, 1);

    public static void InstantiateSprite(PackedScene scene)
    {
        var instance = scene.Instantiate<Node2D>();
        
        instance.GlobalPosition = Main.Player.GlobalPosition;
        if (Main.Player.IsFacingRight) 
            instance.Scale = FlippedH;
        
        Main.Player.GetParent().AddChild(instance);
    }
}