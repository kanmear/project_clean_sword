// ReSharper disable UnusedParameter.Local
namespace ProjectCleanSword.Scripts.Player;

using Godot;
using Interfaces;
using StateMachine.Player;
using StateMachine.Player.ConcreteStates;

public partial class PlayerController : CharacterBody2D, IMovable
{
	[Export] private Timer dashCooldownTimer;
	[Export] private Timer attackCooldownTimer;
	[Export] private Timer attackComboTimer;
    
	[Export] public GhostTrailParticle GhostTrailParticle;
	[Export] public PlayerAnimator PlayerAnimator;
	[Export] public PlatformEdgeDetector PlatformEdgeDetector;
	
	#region IMovable fields

	[Export] public float MovingSpeed { get; set; } = 300.0f;
	[Export] public float DashImpulse = 1600.0f;
	[Export] public float JumpImpulse = -400.0f; //NOTE try to increase it together with gravity for a snappier jump
	[Export] public float SmoothDelta = 14f;
	[Export] private float jumpWindow = 0.1f;
	public bool IsFacingRight { get; set; } = true; 

	#endregion

	#region Internal fields

	private Vector2 velocity;
	private float fDelta;

	private float timeSinceLeftFloor;
	
	private float dashCooldown = 2f;
	private float dashLength = 0.2f;

	private bool isMovable = true;
	private float attackCooldown = 0.3f;
	private float attackComboTimeout = 1.0f;
	private int attackComboCount;

	private bool isAgainstWall;
	private bool isWallkickAvailable = true;

	private bool isRechargedJumpAvailable;
	
	#endregion

	#region StateMachine fields

	[Export] private string currentState;
	private PlayerStateMachine StateMachine { get; set; }	
	public IdlePlayerState IdlePlayerState { get; private set; }	
	public RunningPlayerState RunningPlayerState { get; private set; }	
	public JumpingPlayerState JumpingPlayerState { get; private set; }	
	public FallingPlayerState FallingPlayerState { get; private set; }	
	public DashingPlayerState DashingPlayerState { get; private set; }	
	public AttackingPlayerState AttackingPlayerState { get; private set; }	

	#endregion
	
	public override void _Ready()
	{
		Main.Player = this;

		StateMachine = new PlayerStateMachine();
		IdlePlayerState = new IdlePlayerState(this, StateMachine);
		RunningPlayerState = new RunningPlayerState(this, StateMachine);
		JumpingPlayerState = new JumpingPlayerState(this, StateMachine);
		FallingPlayerState = new FallingPlayerState(this, StateMachine);
		DashingPlayerState = new DashingPlayerState(this, StateMachine);
		AttackingPlayerState = new AttackingPlayerState(this, StateMachine);
		
		StateMachine.Initialize(IdlePlayerState);
	}

	public override void _PhysicsProcess(double delta)
	{
		fDelta = (float) delta;
		
		StateMachine.CurrentState.PhysicsProcess(fDelta);
		currentState = StateMachine.CurrentState.Name.ToString(); //NOTE for debug

		// if (Input.IsActionJustPressed("attack") && !IsOnFloor())
		// {
		// 	bladeAnimator.PlayAttack();
		// }
        
		IsJumpAvailable();
	}

	public override void _Process(double delta)
	{
		StateMachine.CurrentState.FrameProcess((float) delta);
	}

	public void Move(Vector2 velocityValue)
	{
        Velocity = velocityValue;
		MoveAndSlide();
	}

	#region jump state helper methods
    
	public bool IsJumpAvailable()
	{
		if (!IsOnFloor())
		{
			timeSinceLeftFloor += fDelta;
			return timeSinceLeftFloor <= jumpWindow;
		}
		
		timeSinceLeftFloor = 0f;
		return true;
	}
    
	public bool IsWallKickAvailable() => isAgainstWall && isWallkickAvailable;
	public void SetWallkickAvailability(bool value) => isWallkickAvailable = value;

	public bool IsRechargedJumpAvailable() => isRechargedJumpAvailable;
	public void SetRechargedJumpAvailability(bool value) => isRechargedJumpAvailable = value;

	private void OnBackdropTileEntered(Node2D body) => isAgainstWall = true;
	private void OnBackdropTileExited(Node2D body) => isAgainstWall = false;
    
	#endregion

	#region dash state helper methods
    
	public void StartDashTimer() => dashCooldownTimer.Start(dashCooldown);
	public bool IsDashAvailable() => dashCooldownTimer.IsStopped();
	public bool IsDashFinished() => dashCooldownTimer.TimeLeft <= dashCooldown - dashLength;
    
	#endregion

	#region attack state helper methods

	public void SetMovable(bool value) => isMovable = value;
	public bool IsMovable() => isMovable;
    
	public void StartAttackCooldownTimer() => attackCooldownTimer.Start(attackCooldown);
	public bool IsAttackAvailable() => attackCooldownTimer.IsStopped();

	public void StartAttackComboTimer() => attackComboTimer.Start(attackComboTimeout);
	public bool IsComboAvailable() => !attackComboTimer.IsStopped();

	public int IncrementComboCount() =>
		attackComboCount = ++attackComboCount <= 3
			? attackComboCount
			: 1;

	public int GetComboCount() => attackComboCount;
	private void OnAttackAnimationFinished() => StateMachine.CurrentState.OnCustomEvent();

	private void OnComboTimerTimeout() => attackComboCount = 0;

	#endregion
}