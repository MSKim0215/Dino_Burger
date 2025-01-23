using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterState
{
    public enum BehaviourState
    {
        Move, Waiting, Order, OrderSuccess, OrderFailure, MoveSuccess, MoveFailure, InterAction, None
    }

    public void Convert(CharacterController controller);

    public BehaviourState Get();
}

public class MoveState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Move;
    }
}

public class WaitingState : ICharacterState
{
    public async void Convert(CharacterController controller)
    {
        var player = controller as MSKim.Player.PlayerController;
        if(player != null)
        {
            controller.View.PlayAnimation(Get());
            return;
        }

        var guest = controller as MSKim.NonPlayer.GuestController;
        if (guest == null) return;

        var targetRotation = Quaternion.Euler(new(0, 180, 0));
        while (controller != null &&
            Quaternion.Angle(controller.transform.rotation, targetRotation) > 0.1f)
        {
            controller.transform.rotation =
                Quaternion.Slerp(controller.transform.rotation, targetRotation, 15f * Time.deltaTime);

            await UniTask.Yield();
        }

        if (controller == null) return;

        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Waiting;
    }
}

public class OrderState : ICharacterState
{
    public async void Convert(CharacterController controller)
    {
        var guest = controller as MSKim.NonPlayer.GuestController;
        if (guest == null) return;

        var targetRotation = Quaternion.Euler(new(0, 180, 0));
        while (controller != null &&
            Quaternion.Angle(controller.transform.rotation, targetRotation) > 0.1f)
        {
            controller.transform.rotation = 
                Quaternion.Slerp(controller.transform.rotation, targetRotation, 15f * Time.deltaTime);

            await UniTask.Yield();
        }

        if (controller == null) return;

        guest.Order(GetOrderBurger(guest.Data.MinimumToppingCount, guest.Data.MaximumToppingCount), IsOrderStew());
        controller.View.PlayAnimation(Get());
    }

    private List<Utils.CrateType> GetOrderBurger(int minCount, int maxCount)
    {
        var orderIngredients = new List<Utils.CrateType>();
        var toppingCount = UnityEngine.Random.Range(minCount, maxCount);
        var allowList = MSKim.Manager.GameManager.Instance.AllowIncredientList;

        for(int i = 0; i < toppingCount; i++)
        {
            var selectIncredientIndex = UnityEngine.Random.Range(0, allowList.Count);
            orderIngredients.Add(allowList[selectIncredientIndex]);
        }

        return orderIngredients;
    }

    private bool IsOrderStew() => UnityEngine.Random.Range(0, 2) == 0;

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.Order;
    }
}

public class OrderSuccessState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.OrderSuccess;
    }
}

public class OrderFailureState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.OrderFailure;
    }
}

public class MoveSuccessState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.MoveSuccess;
    }
}

public class MoveFailureState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.MoveFailure;
    }
}

public class InterActionState : ICharacterState
{
    public void Convert(CharacterController controller)
    {
        controller.View.PlayAnimation(Get());
    }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.InterAction;
    }
}

public class NoneState : ICharacterState
{
    public void Convert(CharacterController controller) { }

    public ICharacterState.BehaviourState Get()
    {
        return ICharacterState.BehaviourState.None;
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