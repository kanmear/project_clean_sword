namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Godot;
using Scripts.Enemy;

public class EnemyIdleState : EnemyState
{
    private Vector2 velocity;
    private readonly float idleTimeLimit = 0.5f;
    private float currentIdleTime;
    private bool isAttacking;
    
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        Name = StateName.Default;
    }

    public override void EnterState()
    {
        velocity = Enemy.Velocity;
        velocity.X = 0f;
        
        currentIdleTime = 0f;
        isAttacking = false;
    }
    
    public override void PhysicsProcess(float delta)
    {
		velocity.Y += GlobalConstants.Gravity * delta;
        Enemy.Move(velocity);
        
        currentIdleTime += delta;
        if (currentIdleTime >= idleTimeLimit)
        {
            if (isAttacking)
                EnemyStateMachine.ChangeState(Enemy.AttackState);
            else
                EnemyStateMachine.ChangeState(Enemy.ChaseState);
        }
    }

    public override void ExitState()
    {
        Enemy.SetFlippable(true);
    }

    public override void OnPlayerEnter()
    {
        isAttacking = true;
    }

    public override void OnDamage()
    {
        EnemyStateMachine.ChangeState(Enemy.DamagedState);
    }
}