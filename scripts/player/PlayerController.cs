using Godot;

namespace ProjectCleanSword.scripts.player;

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
		
		Animate(velocity);
	}

	private void Animate(Vector2 velocity)
	{
		sprite2D.Play(Mathf.Abs(velocity.X) > 1 ? "running" : "idle");

		if (!IsOnFloor()) 
		{
			if (velocity.Y < 0)
				sprite2D.Play("jumping");
			else if (velocity.Y > 0)
				sprite2D.Play("falling");
		}

		sprite2D.FlipH = velocity.X switch
		{
			> 0 => false,
			< 0 => true,
			_ => sprite2D.FlipH
		};
	}
}