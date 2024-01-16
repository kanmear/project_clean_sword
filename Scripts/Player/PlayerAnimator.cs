using Godot;

namespace ProjectCleanSword.Scripts.Player;

using StateMachine.Player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private PlayerController playerController;
	[Export] private Sprite2D sprite2D;

	public override void _Process(double delta) => Animate();

	private void Animate()
	{
		if (playerController.StateMachine.CurrentState.Name == PlayerState.StateName.Attacking)
		{
			Play("attacking_idle");
			return;
		}

		if (playerController.StateMachine.CurrentState.Name == PlayerState.StateName.Dashing)
		{
			Play("dashing");
			return;
		}

		var velocity = playerController.Velocity;
		var isOnFloor = playerController.IsOnFloor();
		
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