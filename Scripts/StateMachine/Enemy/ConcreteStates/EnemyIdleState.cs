namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Godot;
using Scripts.Enemy;

public class EnemyIdleState : EnemyState
{
    private Vector2 velocity;
    private readonly float idleTimeLimit = 1f;
    private float currentIdleTime;
    
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        velocity = Enemy.Velocity;
        velocity.X = 0f;
        
        currentIdleTime = 0f;
    }
    
    public override void PhysicsProcess(float delta)
    {
		velocity.Y += GlobalConstants.Gravity * delta;
        Enemy.Move(velocity);
        
        currentIdleTime += delta;
        if (currentIdleTime >= idleTimeLimit)
            EnemyStateMachine.ChangeState(Enemy.ChaseState);
    }

    public override void OnDamage()
    {
        EnemyStateMachine.ChangeState(Enemy.DamagedState);
    }
}