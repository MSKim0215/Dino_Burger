public interface ICharacterState
{
    public enum BehaviourState
    {
        Move, Pickup, Waiting, Order
    }

    public void Convert(CharacterController controller);

    public BehaviourState Get();
}

public class MoveState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        UnityEngine.Debug.Log("걷기 시작");
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
        UnityEngine.Debug.Log("기다리기 시작!!");
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
        UnityEngine.Debug.Log("픽업 완료!! 돌아간다!");
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Pickup;
    }
}

public class OrderState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        UnityEngine.Debug.Log("주문 요청!");
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Order;
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