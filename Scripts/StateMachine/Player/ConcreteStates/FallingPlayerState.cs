namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class FallingPlayerState : PlayerState
{
	private Vector2 velocity;
    
    public FallingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
	    Name = StateName.Falling;
    }

    public override void PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("dash") & Player.IsDashReady())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
        
	    if (Player.IsOnFloor())
		    PlayerStateMachine.ChangeState(Player.DefaultPlayerState);
	    
	    velocity = Player.Velocity;
		velocity.Y += GlobalConstants.Gravity * delta;
        
		bool leftInput = Input.IsActionPressed("left");
		bool rightInput = Input.IsActionPressed("right");
		
		if (leftInput ^ rightInput)
		{
			if (Mathf.Abs(Player.Velocity.X) < Player.MovingSpeed) 
				velocity.X = Player.MovingSpeed * (leftInput ? -1 : 1);
			else
				velocity.X = Mathf.MoveToward(velocity.X, 0, Player.SmoothDelta);
		} 
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Player.SmoothDelta);
		}
		
	    Player.Move(velocity);
    }
}