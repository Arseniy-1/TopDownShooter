using System;
using UnityEngine;

public partial class PlayerCollisionHandler : MonoBehaviour
{
    public event Action<IInteractable> CollisionDetected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable))
        {
            CollisionDetected(interactable);    
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent(out IInteractable interactable))
        {
            CollisionDetected(interactable);
        }
    }
}
