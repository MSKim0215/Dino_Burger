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

    [Header("Component")]
    [SerializeField] protected Rigidbody rigid;

    private void Awake()
    {
        SettingState();
    }

    protected virtual void SettingState()
    {
        state = new(this);
    }

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
