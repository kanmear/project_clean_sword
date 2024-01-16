namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class AttackingPlayerState : PlayerState
{
	private Vector2 velocity;
    
	private float timeSinceAttack;
	
	private const float AttackCooldown = 0.5f;
    public AttackingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        Name = StateName.Attacking;
    }

    public override void EnterState()
    {
        velocity = Vector2.Zero;
    }

    public override void ExitState()
    {
        timeSinceAttack = 0f;
    }

    public override void PhysicsProcess(float delta)
    {
        Player.Move(velocity);

        timeSinceAttack += delta;
		if (!(timeSinceAttack >= AttackCooldown)) return;
        
        PlayerStateMachine.ChangeState(Player.DefaultPlayerState);
    }
}