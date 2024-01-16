namespace ProjectCleanSword.Scripts.StateMachine.Player;

using Scripts.Player;

public class PlayerState
{
    protected readonly PlayerController Player;
    protected readonly PlayerStateMachine PlayerStateMachine;

    public StateName Name { get; protected init; }

    protected PlayerState(PlayerController player, PlayerStateMachine playerStateMachine)
    {
        Player = player;
        PlayerStateMachine = playerStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameProcess(float delta) { }
    public virtual void PhysicsProcess(float delta) { }

    public enum StateName
    {
        Default,
        Attacking,
        Jumping,
        Running,
        Falling,
        Dashing
    }
}