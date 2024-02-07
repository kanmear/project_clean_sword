namespace ProjectCleanSword.Scripts.Enemy;

using Godot;

public partial class EnemyAnimator : AnimationPlayer
{
	[Export] private Enemy enemy;
	[Export] private Sprite2D sprite2D;

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

		if (enemy.IsFlippable)
		{
			var x = velocity.X switch
			{
				> 0 => 1,
				< 0 => -1,
				_ => sprite2D.Scale.X
			};
			enemy.IsFacingRight = (int) x == 1;
			sprite2D.Scale = new Vector2(x, 1);
		}
	}
}