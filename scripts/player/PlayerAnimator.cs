using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerAnimator : AnimatedSprite2D
{
	[Export] private CharacterBody2D characterBody2D;

	public override void _Process(double delta) => Animate();

	private void Animate()
	{
		var velocity = characterBody2D.Velocity;
		var isOnFloor = characterBody2D.IsOnFloor();
		
		Play(Mathf.Abs(velocity.X) > 1 ? "running" : "idle");

		if (!isOnFloor) 
		{
			if (velocity.Y < 0)
				Play("jumping");
			else if (velocity.Y > 0)
				Play("falling");
		}

		FlipH = velocity.X switch
		{
			> 0 => false,
			< 0 => true,
			_ => FlipH
		};
	}
}