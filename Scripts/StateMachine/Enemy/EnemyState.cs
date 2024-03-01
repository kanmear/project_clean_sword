namespace ProjectCleanSword.Scripts.StateMachine.Enemy;

using Scripts.Enemy;

public class EnemyState
{
    protected Enemy Enemy;
    protected EnemyStateMachine EnemyStateMachine;
    
    public StateName Name { get; protected init; }

    public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
    {
        Enemy = enemy;
        EnemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameProcess(float delta) { }
    public virtual void PhysicsProcess(float delta) { }
    public virtual void OnDamage() { }
    public virtual void OnPlayerEnter() { } //TODO rework?
    
    public enum StateName
    {
        Default,
        Attacking,
        Damaged,
        Dead,
        Chasing,
    }
}