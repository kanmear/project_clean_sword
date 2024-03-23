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

    public override void EnterState()
    {
        Animator.Play(Name.ToString());
    }

    public override void PhysicsProcess(float delta)
    {
	    if (!Player.IsAbleToJump())
		    PlayerStateMachine.ChangeState(Player.FallingPlayerState);
	    if (Input.IsActionPressed("jump"))
		    PlayerStateMachine.ChangeState(Player.JumpingPlayerState);
        if (Input.IsActionJustPressed("dash") & Player.IsDashReady())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
        
		velocity = Player.Velocity;
		
		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");

		if (leftInput ^ rightInput)
		{
			velocity.X = Player.MovingSpeed * (leftInput ? -1 : 1);
			Player.Move(velocity);
		} 
		else 
			PlayerStateMachine.ChangeState(Player.IdlePlayerState);
    }
}