namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class RunningPlayerState : PlayerState
{
	private Vector2 velocity;
	
    public RunningPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
	    Name = StateName.Running;
    }

    public override void EnterState(object argument)
    {
        Animator.PlayRunning();
    }

    public override void PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("attack") && Player.IsAttackAvailable())
        {
            PlayerStateMachine.ChangeState(Player.AttackingPlayerState);
            return;
        }
        
	    if (!Player.IsJumpAvailable())
	    {
		    PlayerStateMachine.ChangeState(Player.FallingPlayerState);
		    return;
	    }
        
	    if (Input.IsActionPressed("jump"))
	    {
		    PlayerStateMachine.ChangeState(Player.JumpingPlayerState);
		    return;
	    }
        
		if (Input.IsActionJustPressed("dash") && Player.IsDashAvailable())
        {
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
            return;
        }
        
		velocity = Player.Velocity;
		
		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");

		if (leftInput ^ rightInput)
		{
			velocity.X = Player.MovingSpeed * (leftInput ? -1 : 1);
			Player.Move(velocity);
		} 
		else 
			PlayerStateMachine.ChangeState(Player.IdlePlayerState, false);
    }
}