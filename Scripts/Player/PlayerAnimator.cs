using Godot;

namespace ProjectCleanSword.Scripts.Player;

public partial class PlayerAnimator : AnimationPlayer
{
	[Export] private PlayerController playerController;
	[Export] private BladeAnimator bladeAnimator;
    
	[Export] private Sprite2D sprite2D;

	#region player animation stringnames
    
	private static readonly StringName Idle = "Idle";
	// private static readonly StringName IdleStatic = "IdleStatic";
	private static readonly StringName IdleFlipped = "IdleFlipped";
    
	private static readonly StringName Running = "Running";
    
	private static readonly StringName Falling = "Falling";
	private static readonly StringName FallingLoop = "FallingLoop";
	private static readonly StringName FallImpact = "FallImpact";
    
	private static readonly StringName Wallkick = "Wallkick";
	private static readonly StringName RechargedJump = "RechargedJump";

	private static readonly StringName AttackLand = "AttackLand";
	private static readonly StringName AttackAir = "AttackAir";

	#endregion

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
        
		// bladeAnimator.Play(Idle);
	}

	public void PlayFalling()
	{
        Play(Falling);
        Queue(FallingLoop);
	}

	public void PlayWallkickJump()
	{
		SpeedScale = 1.5f;
		Play(Wallkick);
	}
    
	public void PlayRechargedJump()
	{
		SpeedScale = 1.5f;
		Play(RechargedJump); 
	}

	public void PlayLandAttack(int comboCount)
	{
		var animationName = AttackLand + $"{comboCount}";
        
		Play(animationName);
		bladeAnimator.Play(animationName);
	}

	public void PlayAirAttack(int comboCount)
	{
		var animationName = AttackAir + $"{comboCount}";
        
		// Play(animationName);
		bladeAnimator.Play(animationName);
	}

	public void PlayRunning()
	{
		Play(Running);
		// bladeAnimator.Play(Running);
	}

	private void OnCurrentAnimationChanged(StringName animationName)
	{
		if (animationName.Equals(Wallkick) || animationName.Equals(RechargedJump))
			SpeedScale = 1f;
		// if (animationName.Equals(IdleStatic))
		// 	PlayIdle();
		// else if (animationName.Equals(Idle) || animationName.Equals(IdleFlipped))
		// 	Play(IdleStatic);
	}
}