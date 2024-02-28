namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class DashingPlayerState : PlayerState
{
	private Vector2 velocity;
	
	private float impulse;
	private float timeSinceDash;
	
	private const float DashLength = 0.2f;

	public DashingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
	{
		Name = StateName.Dashing;
	}

    public override void EnterState()
    {
	    Player.StartDashTimer();
		timeSinceDash = 0f;
	    
	    velocity = Vector2.Zero;
	    impulse = Player.DashImpulse;
    }

    public override void ExitState()
    {
	    Player.Move(Vector2.Zero);
    }

    public override void PhysicsProcess(float delta)
    {
	    var isOnFloor = Player.IsOnFloor();
	    
	    var movingSpeed = Player.MovingSpeed;

	    velocity.Y = 0f;
		velocity.X = impulse * (Player.IsFacingRight ? 1f : -1f);
		impulse -= delta * 8000f;
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
		if (!(timeSinceDash >= DashLength)) return;
		
		if (isOnFloor) 
			PlayerStateMachine.ChangeState(Player.DefaultPlayerState);
		else
			PlayerStateMachine.ChangeState(Player.FallingPlayerState);
    }
}