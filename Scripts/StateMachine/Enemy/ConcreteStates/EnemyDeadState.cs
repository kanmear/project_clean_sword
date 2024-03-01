using Godot;

namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

public class EnemyDeadState : EnemyState
{
    private Vector2 velocity;
    
    public EnemyDeadState(Scripts.Enemy.Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        Name = StateName.Dead;
    }

    public override void EnterState()
    {
        velocity = Enemy.Velocity;
    }

    public override void PhysicsProcess(float delta)
    {
        if (Enemy.IsOnFloor())
            velocity.X = 0f;
        
		velocity.Y += GlobalConstants.Gravity * delta;
        Enemy.Move(velocity);
    }
}