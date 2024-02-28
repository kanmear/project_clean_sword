using Godot;

namespace ProjectCleanSword.Scripts.Player;

using StateMachine.Player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private PlayerController playerController;
	[Export] private Sprite2D sprite2D;

	//TODO move it out from Process, should just be called once from states mostly
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

		var x = velocity.X switch
		{
			> 0 => 1,
			< 0 => -1,
			_ => sprite2D.Scale.X
		};
		playerController.IsFacingRight = (int) x == 1;
		sprite2D.Scale = new Vector2(x, 1);
	}
}