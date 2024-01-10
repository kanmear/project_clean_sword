namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Godot;
using Scripts.Enemy;

public class EnemyChaseState : EnemyState
{
    private Node2D player;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        player = Main.Player;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameProcess()
    {
    }

    public override void PhysicsProcess()
    {
        float direction = 0f;
        float playerX = player.Transform.Origin.X;
        float enemyX = Enemy.Transform.Origin.X;
        if (playerX > enemyX + 5)
        {
            direction = 1f;
        }
        else if (playerX < enemyX - 5) //TODO 5 here is attackRange or something similar
        {
            direction = -1f;
        }
        
        var velocity = new Vector2(direction * Enemy.MovingSpeed, 0);
        Enemy.Move(velocity);
    }
}