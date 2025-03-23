using Unity.Netcode;
using UnityEngine;

public class BaseManager
{
    protected bool IsInit { get; set; } = false;

    public virtual void Initialize()
    {
        if (IsInit) return;

        IsInit = true;
    }

    public virtual void OnUpdate() { }
}