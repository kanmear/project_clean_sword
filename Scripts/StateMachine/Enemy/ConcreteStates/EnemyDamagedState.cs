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
    }

    public override void EnterState()
    {
        player = Main.Player;
        Enemy.SetFlippable(false);
        
        direction = 0f;
        impulse = 300f;
        
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
        velocity = new Vector2(direction * impulse, -impulse);
        
		impulse -= delta * 2000f;
        if (impulse < 0f)
            EnemyStateMachine.ChangeState(Enemy.IdleState);
        
        Enemy.Move(velocity);
    }
}