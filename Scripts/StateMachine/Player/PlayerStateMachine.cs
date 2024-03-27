namespace ProjectCleanSword.Scripts.StateMachine.Player;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState(null);
    }

    public void ChangeState(PlayerState newState, object argument = null)
    {
        CurrentState.ExitState();
        
        CurrentState = newState;
        CurrentState.EnterState(argument);
    }
}