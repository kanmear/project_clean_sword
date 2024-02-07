namespace ProjectCleanSword.Scripts.Enemy;

using Godot;
using Interfaces;
using StateMachine.Enemy;
using StateMachine.Enemy.ConcreteStates;

public partial class Enemy : CharacterBody2D, IDamageable, IMovable
{
    #region IDamageable fields

    [Export] public int MaxHealth { get; set; }

    public int CurrentHealth { get; set; }

    #endregion
    #region IMovable fields 

    public bool IsFacingRight { get; set; } = true;
    [Export] public float MovingSpeed { get; set; } = 200f;

    #endregion
    #region StateMachine fields

    public EnemyStateMachine StateMachine { get; set; }
    public EnemyIdleState IdleState { get; set; }
    public EnemyChaseState ChaseState { get; set; }
    public EnemyAttackState AttackState { get; set; }
    public EnemyDamagedState DamagedState { get; set; }

    #endregion

    public override void _Ready()
    {
        base._Ready();
        
        CurrentHealth = MaxHealth;

        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);
        DamagedState = new EnemyDamagedState(this, StateMachine);
        
        StateMachine.Initialize(ChaseState);
    }

    public override void _PhysicsProcess(double delta)
    {
        StateMachine.CurrentState.PhysicsProcess((float) delta);
    }

    public override void _Process(double delta)
    {
        StateMachine.CurrentState.FrameProcess((float) delta);
    }

    public void Move(Vector2 velocity)
    {
        Velocity = velocity;
        MoveAndSlide();
    }

    public void Damage(int amount)
    {
        CurrentHealth -= amount;
        StateMachine.CurrentState.OnDamage();
        if (CurrentHealth <= 0) Death();
    }

    public void Death()
    {
        QueueFree();
    }
}