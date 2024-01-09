using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float speed = 300.0f;
	[Export] private float jumpVelocity = -400.0f;
	[Export] private float smoothDelta = 14f;
	[Export] private float jumpWindow = 0.1f;
	private float timeSinceLeftFloor;
	private bool isJumpUsed;
	
	public bool IsAttacking;
	private float attackCooldown = 0.5f;
	private float timeSinceAttack;

	//NOTE Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		var fDelta = (float) delta;
		
		HandleVerticalMovement(fDelta);
		HandleHorizontalMovement();
		HandleAttacking(fDelta);

		MoveAndSlide();
	}

	private void HandleAttacking(float delta)
	{
		if (!IsAttacking && IsOnFloor() && Input.IsActionJustPressed("attack")) 
			IsAttacking = true;

		if (!IsAttacking) return;
		
		Velocity = Vector2.Zero;
		timeSinceAttack += delta;
		
		if (!(timeSinceAttack >= attackCooldown)) return;
			
		IsAttacking = false;
		timeSinceAttack = 0f;
	}

	private void HandleVerticalMovement(float delta)
	{
		var velocity = Velocity;
		
		var isOnFloor = IsOnFloor();
		var isAbleToJump = IsAbleToJump(delta, isOnFloor);

		if (!isOnFloor) velocity.Y += gravity * delta;
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
		var velocity = Velocity;
		
		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");
		
		if (leftInput ^ rightInput)
			velocity.X = speed * (leftInput ? -1 : 1);
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, smoothDelta);
		
		Velocity = velocity;
	}

	private bool IsAbleToJump(float delta, bool isOnFloor)
	{
		if (!isOnFloor)
		{
			timeSinceLeftFloor += delta;
			return timeSinceLeftFloor <= jumpWindow;
		}

		timeSinceLeftFloor = 0f;
		return true;
	}
}