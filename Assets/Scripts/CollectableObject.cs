using System;
using UnityEngine;

public abstract class CollectableObject : MonoBehaviour
{
    public event Action<CollectableObject> OnDestroyed;

    public void RaiseDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public abstract void Activate();
}
