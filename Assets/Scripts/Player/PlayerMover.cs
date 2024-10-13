using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private InputHandler _inputHandler;

    private Animator _animator;
    private string _runningTrigger = "IsRunning";
    private Rigidbody2D _rigidbody2D;

    public float HorizontalSpeed => _rigidbody2D.velocity.x;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float currentHorizontalSpeed = _inputHandler.HorizontalDirection * _speed;
        float currentVerticalSpeed = _inputHandler.VerticalDirection * _speed;

        _rigidbody2D.velocity = new Vector2(currentHorizontalSpeed, currentVerticalSpeed);
        //_animator.SetBool(_runningTrigger, currentHorizontalSpeed != 0);
    }
}
