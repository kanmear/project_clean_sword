using Godot;
using ProjectCleanSword.Scripts.Player;

namespace ProjectCleanSword.Scripts.StateMachine.Player.ConcreteStates;

public class AttackingPlayerState : PlayerState
{
	private Vector2 velocity;
    
    public AttackingPlayerState(PlayerController player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
        Name = StateName.Attacking;
    }

    public override void EnterState(object argument)
    {
        Player.StartAttackCooldownTimer();
        
        Player.StartAttackComboTimer();
        var comboCount = Player.GetComboCount();
        if (comboCount == 0 || Player.IsComboAvailable())
            comboCount = Player.IncrementComboCount();

        velocity = Player.Velocity;
        velocity.X = Player.MovingSpeed * (0.3f * comboCount)
                     * (Player.IsFacingRight ? 1 : -1);

        Animator.PlayLandAttack(comboCount);
    }

    public override void PhysicsProcess(float delta)
    {
        velocity.X = Mathf.MoveToward(velocity.X, 0, Player.SmoothDelta);
        Player.Move(velocity);
        
        if (!Player.IsOnFloor())
        {
            PlayerStateMachine.ChangeState(Player.FallingPlayerState);
            return;
        }
        
        if (!Player.IsAttackAvailable()) return;
        
        //NOTE copy-paste from IdleState
        if (Input.IsActionJustPressed("dash") && Player.IsDashAvailable())
        {
            PlayerStateMachine.ChangeState(Player.DashingPlayerState);
            return;
        }

        if (Input.IsActionJustPressed("jump"))
        {
            PlayerStateMachine.ChangeState(Player.JumpingPlayerState);
            return;
        }

        if (Input.IsActionPressed("left") ^ Input.IsActionPressed("right"))
            PlayerStateMachine.ChangeState(Player.RunningPlayerState);
    }

    //NOTE is fired when attack animation is finished
    public override void OnCustomEvent() => PlayerStateMachine.ChangeState(Player.IdlePlayerState);
}