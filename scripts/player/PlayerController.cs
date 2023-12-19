using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float speed = 300.0f;
	[Export] private float jumpVelocity = -400.0f;
	[Export] private float smoothDelta = 14f;
	[Export] private float jumpWindow = 0.1f;
	private float timeSinceLeftFloor;
	private bool jumpUsed;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		var isAbleToJump = IsAbleToJump((float)delta);

		if (!IsOnFloor()) velocity.Y += gravity * (float)delta;
		else jumpUsed = false;

		if (!jumpUsed && Input.IsActionJustPressed("jump") && isAbleToJump)
		{
			velocity.Y = jumpVelocity;
			jumpUsed = true;
		}

		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");
		
		if (leftInput ^ rightInput)
			velocity.X = speed * (leftInput ? -1 : 1);
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, smoothDelta);

		Velocity = velocity;
		MoveAndSlide();
	}

	private bool IsAbleToJump(float delta)
	{
		var isOnFloor = IsOnFloor();
		if (!isOnFloor)
		{
			timeSinceLeftFloor += delta;
			return timeSinceLeftFloor <= jumpWindow;
		}

		timeSinceLeftFloor = 0f;
		return true;
	}
}