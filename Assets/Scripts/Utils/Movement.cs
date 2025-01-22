using System.Collections.Generic;
using UnityEngine;

public interface ICharacterMove
{
    public void Move();
    public Vector3 GetVelocity();
    public void MovePosition();
    public void MoveRotation();
}

public abstract class CharacterController : PoolAble, ICharacterMove
{
    protected StateController state;
    protected Dictionary<ICharacterState.BehaviourState, ICharacterState> stateDict = new();

    private void Awake()
    {
        state = new(this);
        SettingState();
    }
    
    protected abstract void SettingState();

    public virtual Vector3 GetVelocity() => Vector3.zero;

    protected void ChangeState(ICharacterState.BehaviourState changeState) => state.ChangeState(stateDict[changeState]);

    public virtual void Move()
    {
        MovePosition();
        MoveRotation();
    }

    public abstract void MovePosition();

    public abstract void MoveRotation();
}
