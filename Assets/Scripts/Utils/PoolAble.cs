using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolAble : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }

    public virtual void Release()
    {
        Pool.Release(gameObject);
    }
}
