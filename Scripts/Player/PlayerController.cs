namespace ProjectCleanSword.Scripts.Player;

using Godot;
using Interfaces;
using StateMachine.Player;
using StateMachine.Player.ConcreteStates;

public partial class PlayerController : CharacterBody2D, IMovable
{
	[Export] private BladeAnimator bladeAnimator;
	[Export] private Timer dashCooldownTimer;
    
	[Export] public GhostTrailParticle GhostTrailParticle;
	[Export] public PlayerAnimator PlayerAnimator;
	
	#region IMovable fields

	[Export] public float MovingSpeed { get; set; } = 300.0f;
	[Export] public float DashImpulse = 1600.0f;
	[Export] public float JumpImpulse = -400.0f;
	[Export] public float SmoothDelta = 14f;
	[Export] private float jumpWindow = 0.1f;
	public bool IsFacingRight { get; set; } = true; 

	#endregion

	#region Internal fields

	private Vector2 velocity;
	private float fDelta;

	private float timeSinceLeftFloor;
	private float attackCooldown = 0.2f;
	private float timeSinceLastAttack;
	
	private float dashCooldown = 2f;
	private float dashLength = 0.2f;
	
	#endregion

	#region StateMachine fields

	[Export] private string currentState;
	private PlayerStateMachine StateMachine { get; set; }	
	public IdlePlayerState IdlePlayerState { get; private set; }	
	public RunningPlayerState RunningPlayerState { get; private set; }	
	public JumpingPlayerState JumpingPlayerState { get; private set; }	
	public FallingPlayerState FallingPlayerState { get; private set; }	
	public DashingPlayerState DashingPlayerState { get; private set; }	

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
		
		StateMachine.Initialize(IdlePlayerState);
	}

	public override void _PhysicsProcess(double delta)
	{
		fDelta = (float) delta;
		
		StateMachine.CurrentState.PhysicsProcess(fDelta);
		currentState = StateMachine.CurrentState.Name.ToString(); //NOTE for debug

		if (Input.IsActionJustPressed("attack"))
		{
			bladeAnimator.PlayAttack();
		}

		IsAbleToJump();
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

	public bool IsAbleToJump()
	{
		if (!IsOnFloor())
		{
			timeSinceLeftFloor += fDelta;
			return timeSinceLeftFloor <= jumpWindow;
		}
		
		timeSinceLeftFloor = 0f;
		return true;
	}

	public void StartDashTimer() => dashCooldownTimer.Start(dashCooldown);
	public bool IsDashReady() => dashCooldownTimer.IsStopped();
	public bool IsDashFinished() => dashCooldownTimer.TimeLeft <= dashCooldown - dashLength;
}