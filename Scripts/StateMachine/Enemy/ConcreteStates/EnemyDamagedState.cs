namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Godot;
using Scripts.Enemy;

public class EnemyDamagedState : EnemyState
{
    private Node2D player;
    private Vector2 velocity;
    private float direction;
    private float impulse = 300f;
    
    public EnemyDamagedState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        Name = StateName.Damaged;
    }

    public override void EnterState()
    {
        player = Main.Player;
        Enemy.SetFlippable(false);
        
        direction = 0f;
        impulse = 350f;
        
        float playerX = player.Transform.Origin.X;
        float enemyX = Enemy.Transform.Origin.X;
        if (playerX > enemyX)
        {
            direction = -1f;
        }
        else 
        {
            direction = 1f;
        }
    }

    public override void PhysicsProcess(float delta)
    {
        velocity = new Vector2(direction * impulse, -impulse + 200f);
        
        Enemy.Move(velocity);
        
		impulse -= delta * 2000f;
        if (impulse < 10f)
            impulse = 10f;

        if (Enemy.CurrentHealth <= 0)
        {
            EnemyStateMachine.ChangeState(Enemy.DeadState);
        }
        else if (Enemy.IsOnFloor())
        {
            EnemyStateMachine.ChangeState(Enemy.IdleState);
        }
        
    }
}