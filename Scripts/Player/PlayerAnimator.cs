using System;
using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private PlayerController playerController;
	[Export] private Sprite2D sprite2D;

	private static readonly StringName Idle = "Idle";
	private static readonly StringName IdleStatic = "IdleStatic";
	private static readonly StringName IdleFlipped = "IdleFlipped";
	private static readonly StringName Falling = "Falling";
	private static readonly StringName FallingLoop = "FallingLoop";
	private static readonly StringName FallImpact = "FallImpact";

	public override void _Process(double delta)
	{
		//NOTE weird way to handle this?
		var x = playerController.Velocity.X switch
		{
			> 0 => 1,
			< 0 => -1,
			_ => sprite2D.Scale.X
		};
        
		playerController.IsFacingRight = (int) x == 1;
		sprite2D.Scale = new Vector2(x, 1);
	}

	public void PlayIdle(bool enteredFromAir = false)
	{
		var idleAnimation = playerController.IsFacingRight
			? IdleFlipped
			: Idle;
		
		if (enteredFromAir)
		{
            Play(FallImpact);
            Queue(idleAnimation);
		}
		else
            Play(idleAnimation);
	}

	public void PlayFalling()
	{
        Play(Falling);
        Queue(FallingLoop);
	}

	private void OnAnimationFinished(StringName animationName)
	{
		// if (animationName.Equals(IdleStatic))
		// 	PlayIdle();
		// else if (animationName.Equals(Idle) || animationName.Equals(IdleFlipped))
		// 	Play(IdleStatic);
	}
}