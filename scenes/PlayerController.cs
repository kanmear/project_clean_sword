using Godot;

namespace ProjectCleanSword.scenes;

public partial class PlayerController : CharacterBody2D
{
	[Export] 
	private float speed = 300.0f;
	[Export] 
	private float jumpVelocity = -400.0f;
	[Export] 
	private float smoothDelta = 14f;

	private AnimatedSprite2D sprite2D;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	private float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		sprite2D = GetChild<AnimatedSprite2D>(0);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		sprite2D.Play(Mathf.Abs(velocity.X) > 1 ? "running" : "idle");
		if (Mathf.Abs(velocity.Y) > 1)
			sprite2D.Play("jumping");

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = jumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, smoothDelta);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Handle sprite rotation
		sprite2D.FlipH = velocity.X switch
		{
			> 0 => false,
			< 0 => true,
			_ => sprite2D.FlipH
		};
	}
}