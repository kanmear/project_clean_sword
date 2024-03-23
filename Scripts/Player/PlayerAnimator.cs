using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private PlayerController playerController;
	[Export] private Sprite2D sprite2D;

	public override void _Process(double delta)
	{
		var x = playerController.Velocity.X switch
		{
			> 0 => 1,
			< 0 => -1,
			_ => sprite2D.Scale.X
		};
        
		playerController.IsFacingRight = (int) x == 1;
		sprite2D.Scale = new Vector2(x, 1);
	}
}