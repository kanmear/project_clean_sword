using ProjectCleanSword.Scripts.Effects;

namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

using Godot;
using Scripts.Player;

public class JumpingPlayerState : PlayerState
{
	private Vector2 velocity;
    
    public JumpingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
	    Name = StateName.Jumping;
    }

    public override void EnterState(object argument)
    {
	    velocity = Player.Velocity;
	    velocity.Y = Player.JumpImpulse;
        
        Animator.Play(Name.ToString());

        EffectsHandler.InstantiateSprite(EffectsHandler.PlayerJumpVfx);
    }

    public override void PhysicsProcess(float delta)
    {
        if (Input.IsActionJustPressed("dash") && Player.IsDashReady())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
        
	    if (velocity.Y >= 0)
		    PlayerStateMachine.ChangeState(Player.FallingPlayerState);
	    
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