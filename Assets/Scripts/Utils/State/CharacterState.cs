using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

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
    public async void Convert(CharacterController controller)
    {
        var guest = controller as MSKim.NonPlayer.GuestController;
        if (guest == null) return;

        UnityEngine.Debug.Log("기다리기 시작!!");
        var targetRotation = Quaternion.Euler(new(0, 180, 0));
        while (Quaternion.Angle(controller.transform.rotation, targetRotation) > 0.1f)
        {
            controller.transform.rotation =
                Quaternion.Slerp(controller.transform.rotation, targetRotation, 15f * Time.deltaTime);

            await UniTask.Yield();
        }
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
    public async void Convert(CharacterController controller)
    {
        var guest = controller as MSKim.NonPlayer.GuestController;
        if (guest == null) return;

        var targetRotation = Quaternion.Euler(new(0, 180, 0));
        while (Quaternion.Angle(controller.transform.rotation, targetRotation) > 0.1f)
        {
            controller.transform.rotation = 
                Quaternion.Slerp(controller.transform.rotation, targetRotation, 15f * Time.deltaTime);

            await UniTask.Yield();
        }

        guest.Order(GetOrderBurger(guest.Data.MinimumToppingCount, guest.Data.MaximumToppingCount), IsOrderStew());
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