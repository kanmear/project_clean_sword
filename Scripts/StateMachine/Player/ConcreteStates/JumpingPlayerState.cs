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

	    if (argument != null)
	    {
			if ((bool)argument)
			{
				Player.SetWallkickAvailability(false);
				
				Animator.PlayWallkickJump();
				EffectsHandler.InstantiateSprite(EffectsHandler.PlayerWallkickVfx);
			}
			else
			{
				Player.SetRechargedJumpAvailability(false);
				
				Animator.PlayRechargedJump();
				EffectsHandler.InstantiateSprite(EffectsHandler.PlayerRechargedJumpVfx);
			}
	    }
	    else
	    {
			Animator.Play(Name.ToString());
			EffectsHandler.InstantiateSprite(EffectsHandler.PlayerJumpVfx);
	    }
    }

    public override void PhysicsProcess(float delta)
    {
	    if (Input.IsActionJustPressed("jump"))
	    {
		    if (Player.IsWallKickAvailable())
		    {
				Player.SetWallkickAvailability(false);
				
				velocity.Y = Player.JumpImpulse;
				
				Animator.PlayWallkickJump();
				EffectsHandler.InstantiateSprite(EffectsHandler.PlayerWallkickVfx);
		    }
		    else if (Player.IsRechargedJumpAvailable())
		    {
				Player.SetRechargedJumpAvailability(false);
				
				velocity.Y = Player.JumpImpulse;
				
				Animator.PlayRechargedJump();
				EffectsHandler.InstantiateSprite(EffectsHandler.PlayerRechargedJumpVfx);
		    }
	    }
        
	    else if (Input.IsActionJustPressed("dash") && Player.IsDashAvailable())
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);

	    else if (velocity.Y >= 0)
	    {
		    PlayerStateMachine.ChangeState(Player.FallingPlayerState);
		    return;
	    }
	    
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
        
        if (Input.IsActionJustPressed("attack") && Player.IsAttackAvailable())
        {
            PlayerStateMachine.ChangeState(Player.AttackingPlayerState);
        }
    }
}