using Godot;

namespace ProjectCleanSword.scripts.player;

public partial class PlayerController : CharacterBody2D
{
	[Export] private float speed = 300.0f;
	[Export] private float jumpVelocity = -400.0f;
	[Export] private float smoothDelta = 14f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		//TODO make a wrapper for IsOnFloor for a better jump
		if (!IsOnFloor()) velocity.Y += gravity * (float)delta;

		if (Input.IsActionJustPressed("jump") && IsOnFloor()) velocity.Y = jumpVelocity;

		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");
		
		if (leftInput ^ rightInput)
			velocity.X = speed * (leftInput ? -1 : 1);
		else
			velocity.X = Mathf.MoveToward(Velocity.X, 0, smoothDelta);

		Velocity = velocity;
		MoveAndSlide();
	}
}