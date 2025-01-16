using UnityEngine;

public interface ICharacterMove
{
    public void Move();
    public Vector3 GetVelocity();
    public void MovePosition();
    public void MoveRotation();
}

public abstract class CharacterController : MonoBehaviour, ICharacterMove
{
    [Header("Character Settings")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;

    public virtual Vector3 GetVelocity() => Vector3.zero;

    public virtual void Move()
    {
        MovePosition();
        MoveRotation();
    }

    public abstract void MovePosition();

    public abstract void MoveRotation();
}
