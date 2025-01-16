using UnityEngine;

public interface ICharacterMove
{
    public void Move();
    public Vector3 GetVelocity();
    public void MovePosition(Vector3 velocity);
    public void MoveRotation(Vector3 velocity);
}

public abstract class CharacterController : MonoBehaviour, ICharacterMove
{
    protected float moveSpeed;
    protected float rotateSpeed;

    public abstract Vector3 GetVelocity();

    public virtual void Move()
    {
        var velocity = GetVelocity();
        MovePosition(velocity);
        MoveRotation(velocity);
    }

    public abstract void MovePosition(Vector3 velocity);

    public void MoveRotation(Vector3 velocity)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), rotateSpeed * Time.deltaTime);
    }
}
