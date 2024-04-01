using System;

namespace ProjectCleanSword.Scripts.StateMachine.Player;

using Scripts.Player;

public class PlayerState
{
    protected readonly PlayerController Player;
    protected readonly PlayerAnimator Animator;
    protected readonly PlayerStateMachine PlayerStateMachine;

    public StateName Name { get; protected init; }

    protected PlayerState(PlayerController player, PlayerStateMachine playerStateMachine)
    {
        Player = player;
        Animator = player.PlayerAnimator;
        PlayerStateMachine = playerStateMachine;
    }

    public virtual void EnterState(object argument) { }
    public virtual void ExitState() { }
    public virtual void FrameProcess(float delta) { }
    public virtual void PhysicsProcess(float delta) { }
    public virtual void OnAnimationFinished() { }

    public enum StateName
    {
        Idle,
        Jumping,
        Running,
        Falling,
        Dashing,
        Attacking
    }
}