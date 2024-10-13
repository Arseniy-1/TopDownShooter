using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private string _runningTrigger = "IsRunning";

    public Rigidbody2D Rigidbody2D { get; private set; }
    public Transform Transform { get; private set; }

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Transform = transform;
    }

    public void Run(float currentHorrizontalSpeed)
    {
        _animator.SetBool(_runningTrigger, currentHorrizontalSpeed != 0);
    }
}
