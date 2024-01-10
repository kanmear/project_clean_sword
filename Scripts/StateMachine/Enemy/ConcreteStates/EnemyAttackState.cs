namespace ProjectCleanSword.Scripts.StateMachine.Enemy.ConcreteStates;

using Scripts.Enemy;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }
}