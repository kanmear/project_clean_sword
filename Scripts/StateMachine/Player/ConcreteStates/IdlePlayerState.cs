using ProjectCleanSword.Scripts.Effects;

namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class IdlePlayerState : PlayerState
{
	private Vector2 velocity;
    
    public IdlePlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        Name = StateName.Idle;
    }

    public override void EnterState(object argument)
    {
        Player.SetWallkickAvailability(true);
        Player.SetRechargedJumpAvailable(false);
        
        var enteredFromAir = argument != null && (bool)argument;
        
        velocity = Vector2.Zero;

        Animator.PlayIdle(enteredFromAir);
        
        if (enteredFromAir) 
            EffectsHandler.InstantiateSprite(EffectsHandler.PlayerLandVfx);
    }

    public override void PhysicsProcess(float delta)
    {
        Player.Move(velocity);

        if (!Player.IsOnFloor())
        {
            PlayerStateMachine.ChangeState(Player.FallingPlayerState);
            return;
        }

        if (Input.IsActionJustPressed("dash") && Player.IsDashReady())
        {
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            PlayerStateMachine.ChangeState(Player.JumpingPlayerState);
            return;
        }

        if (Input.IsActionPressed("left") ^ Input.IsActionPressed("right"))
            PlayerStateMachine.ChangeState(Player.RunningPlayerState);
    }
}