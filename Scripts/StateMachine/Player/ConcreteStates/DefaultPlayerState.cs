namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class DefaultPlayerState : PlayerState
{
	private Vector2 velocity;
    
    public DefaultPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        Name = StateName.Default;
    }

    public override void EnterState()
    {
        velocity = Vector2.Zero;
    }

    public override void PhysicsProcess(float delta)
    {
        Player.Move(velocity);

        if (!Player.IsOnFloor())
            PlayerStateMachine.ChangeState(Player.FallingPlayerState);

        if (Input.IsActionJustPressed("dash") & Player.IsDashReady())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
        if (Input.IsActionJustPressed("dash") & Player.IsDashReady())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
        
        if (Input.IsActionJustPressed("jump"))
            PlayerStateMachine.ChangeState(Player.JumpingPlayerState);

        if (Input.IsActionPressed("left") ^ Input.IsActionPressed("right"))
            PlayerStateMachine.ChangeState(Player.RunningPlayerState);
    }
}