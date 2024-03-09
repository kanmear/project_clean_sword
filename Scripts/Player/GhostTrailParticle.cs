using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class GhostTrailParticle : CpuParticles2D
{
    [Export] private Sprite2D playerSprite;

    public override void _Ready() => Enable(false);

    public override void _PhysicsProcess(double delta)
    {
        var playerSpriteTexture = playerSprite.Texture;

        // crop current frame from sprite sheet
        var currentFrame = new AtlasTexture();
        currentFrame.Atlas = playerSpriteTexture;
        currentFrame.Region = 
            new Rect2(new Vector2(playerSprite.Frame * 16, 0), new Vector2(16, 16));
        
        // flip image if necessary
        ((ShaderMaterial)Material).SetShaderParameter("isFlipped", playerSprite.Scale.X < 0);

        // non-optimal way without shaders
        // var image = currentFrame.GetImage();
        // if (playerSprite.Scale.X < 0)
        //     image.FlipX();
        // var texture = new ImageTexture();
        // texture.SetImage(image);
        
        Texture = currentFrame;
    }

    public void Enable(bool isEnabled)
    {
        SetPhysicsProcess(isEnabled);
        Emitting = isEnabled;
    }
}