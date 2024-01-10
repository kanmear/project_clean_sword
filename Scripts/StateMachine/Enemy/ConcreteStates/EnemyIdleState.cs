namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Scripts.Enemy;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
}