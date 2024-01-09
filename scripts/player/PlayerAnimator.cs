using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private CharacterBody2D characterBody2D;
	[Export] private Sprite2D sprite2D;

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

		sprite2D.FlipH = velocity.X switch
		{
			> 0 => false,
			< 0 => true,
			_ => sprite2D.FlipH
		};
	}
}