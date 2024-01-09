using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerController : CharacterBody2D
{
	#region MovementParameters

	[Export] private float speed = 300.0f;
	[Export] private float dashSpeed = 900.0f;
	[Export] private float jumpVelocity = -400.0f;
	[Export] private float smoothDelta = 14f;
	[Export] private float jumpWindow = 0.1f;

	#endregion

	[Export] private Sprite2D sprite2D;

	#region InternalFields

	private Vector2 velocity;
	private bool isOnFloor;
	private float fDelta;

	private float timeSinceLeftFloor;
	private bool isJumpUsed;
	
	private float attackCooldown = 0.5f;
	private float timeSinceAttack;

	private float timeSinceDash;
	private float impulse;
	private float dashCooldown = 0.6f;

	private bool isControllable = true;

	#endregion
	
	//TODO to be replaced with states
	public bool IsAttacking;
	public bool IsDashing;

	//NOTE Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;
		isOnFloor = IsOnFloor();
		fDelta = (float) delta;
		
		HandleVerticalMovement();
		HandleHorizontalMovement();

		HandleDashing();
		
		HandleAttacking();

		MoveAndSlide();
	}

	private void HandleDashing()
	{
		if (isOnFloor && isControllable && Input.IsActionJustPressed("dash"))
		{
			isControllable = false;
			IsDashing = true;
			impulse = dashSpeed;
		}

		if (IsDashing)
		{
			velocity.X = impulse * (sprite2D.FlipH ? -1f : 1f);
			impulse -= fDelta * 2000f;
			if (impulse <= speed)
			{
				if (isOnFloor)
				{
					if (impulse <= 0) impulse = 0;
				}
				else impulse = speed;
			}
			
			Velocity = velocity;
			
			timeSinceDash += fDelta;
			if (!(timeSinceDash >= dashCooldown)) return;
			
			isControllable = true;
			IsDashing = false;
			timeSinceDash = 0f;
		}
	}

	private void HandleAttacking()
	{
		if (!IsAttacking && IsOnFloor() && Input.IsActionJustPressed("attack")) 
			IsAttacking = true;

		if (!IsAttacking) return;
		
		Velocity = Vector2.Zero;
		timeSinceAttack += fDelta;
		
		if (!(timeSinceAttack >= attackCooldown)) return;
			
		IsAttacking = false;
		timeSinceAttack = 0f;
	}

	private void HandleVerticalMovement()
	{
		var isAbleToJump = IsAbleToJump();

		if (!isOnFloor) velocity.Y += gravity * fDelta;
		else isJumpUsed = false;

		if (!isJumpUsed && Input.IsActionJustPressed("jump") && isAbleToJump)
		{
			velocity.Y = jumpVelocity;
			isJumpUsed = true;
		}
		
		Velocity = velocity;
	}

	private void HandleHorizontalMovement()
	{
		velocity = Velocity;
		
		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");
		
		if (leftInput ^ rightInput)
			velocity.X = speed * (leftInput ? -1 : 1);
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, smoothDelta);
		
		Velocity = velocity;
	}

	private bool IsAbleToJump()
	{
		if (!isOnFloor)
		{
			timeSinceLeftFloor += fDelta;
			return timeSinceLeftFloor <= jumpWindow;
		}

		timeSinceLeftFloor = 0f;
		return true;
	}
}