namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Godot;
using Scripts.Enemy;

public class EnemyAttackState : EnemyState
{
    private Vector2 velocity;
    
	private float timeSinceAttack;
	private const float AttackCooldown = 0.7f;
    
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        Name = StateName.Attacking;
    }

    public override void EnterState()
    {
        velocity = Vector2.Zero;
        
        timeSinceAttack = 0f;
    }

    public override void PhysicsProcess(float delta)
    {
        Enemy.Move(velocity);

        timeSinceAttack += delta;
        if (timeSinceAttack >= AttackCooldown)
            EnemyStateMachine.ChangeState(Enemy.IdleState);
    }
}