namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class DashingPlayerState : PlayerState
{
	private Vector2 velocity;
	
	private float impulse;
	
	public DashingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
	{
		Name = StateName.Dashing;
	}

    public override void EnterState()
    {
	    Player.StartDashTimer();
	    
	    velocity = Vector2.Zero;
	    impulse = Player.DashImpulse;
        
	    Player.GhostTrailParticle.Enable(true);
        
        Animator.Play(Name.ToString());
    }

    public override void ExitState()
    {
	    Player.Move(Vector2.Zero);
        
	    Player.GhostTrailParticle.Enable(false);
    }

    public override void PhysicsProcess(float delta)
    {
	    var isOnFloor = Player.IsOnFloor();
	    
	    var movingSpeed = Player.MovingSpeed;

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
	    
		if (!Player.IsDashFinished()) return;
		
		if (isOnFloor) 
			PlayerStateMachine.ChangeState(Player.IdlePlayerState);
		else
			PlayerStateMachine.ChangeState(Player.FallingPlayerState);
    }
}