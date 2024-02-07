namespace ProjectCleanSword.Scripts.Enemy;

using Godot;

public partial class EnemyAnimator : AnimatedSprite2D
{
	[Export] private Enemy enemy;

	public override void _Process(double delta) => Animate();

	private void Animate()
	{
		var velocity = enemy.Velocity;
		var isOnFloor = enemy.IsOnFloor();
		
		Play(Mathf.Abs(velocity.X) > 1 ? "running" : "idle");

		if (!isOnFloor) 
		{
			if (velocity.Y < 0)
				Play("jumping");
			else if (velocity.Y > 0)
				Play("falling");
		}

		FlipH = velocity.X switch
		{
			> 0 => false,
			< 0 => true,
			_ => FlipH
		};
		enemy.IsFacingRight = !FlipH;
	}
}