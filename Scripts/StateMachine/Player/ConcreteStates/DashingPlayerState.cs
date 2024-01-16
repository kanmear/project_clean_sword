namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class DashingPlayerState : PlayerState
{
	private Vector2 velocity;
	
	private float impulse;
	private float timeSinceDash;
	
	private const float DashCooldown = 0.6f;

	public DashingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
	{
		Name = StateName.Dashing;
	}

    public override void EnterState()
    {
	    velocity = Player.Velocity;
	    impulse = Player.DashImpulse;
    }

    public override void ExitState()
    {
		timeSinceDash = 0f;
    }

    public override void PhysicsProcess(float delta)
    {
	    var isOnFloor = Player.IsAbleToJump();
	    
		if (!isOnFloor) 
			PlayerStateMachine.ChangeState(Player.FallingPlayerState);
	    
	    if (Input.IsActionPressed("jump"))
		    PlayerStateMachine.ChangeState(Player.JumpingPlayerState);
	    
	    var movingSpeed = Player.MovingSpeed;
	    
		velocity.X = impulse * (Player.IsFacingRight ? 1f : -1f);
		impulse -= delta * 2000f;
		if (impulse <= movingSpeed)
		{
			if (isOnFloor)
			{
				if (impulse <= 0) impulse = 0;
			}
			else impulse = movingSpeed;
		}
		
		Player.Move(velocity);
		
		timeSinceDash += delta;
		if (!(timeSinceDash >= DashCooldown)) return;
		
		if (isOnFloor) 
			PlayerStateMachine.ChangeState(Player.DefaultPlayerState);
		else
			PlayerStateMachine.ChangeState(Player.FallingPlayerState);
    }
}