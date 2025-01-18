public interface ICharacterState
{
    public enum BehaviourState
    {
        Move, Pickup, Waiting
    }

    public void Convert(CharacterController controller);

    public BehaviourState Get();
}

public class MoveState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        UnityEngine.Debug.Log("걷기");
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Move;
    }
}

public class WaitingState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        UnityEngine.Debug.Log("기다리기");
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Waiting;
    }
}

public class PickupState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        UnityEngine.Debug.Log("픽업하러가기");
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Pickup;
    }
}

public class StateController
{
    private readonly CharacterController characterController;

    public ICharacterState CurrentState { get; private set; }

    public StateController(CharacterController characterController)
    {
        this.characterController = characterController;
    }

    public void ChangeState(ICharacterState nextState)
    {
        if (nextState == CurrentState) return;

        CurrentState = nextState;
        CurrentState.Convert(characterController);
    }
}