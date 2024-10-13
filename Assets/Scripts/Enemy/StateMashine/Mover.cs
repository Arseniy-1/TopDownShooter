using UnityEngine;

public class Mover : State
{
    [SerializeField] private float _speed;
    //[SerializeField] private Flipper flipper;

    private Transform _currentTarget;
    private Animator _animator;
    private string _runningTrigger = "IsRunning";
    private Rigidbody2D _rigidbody2D;

    public bool HasTarget => _currentTarget != null;
    public float HorizontalSpeed => _rigidbody2D.velocity.x;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnUpdate()
    {
        if (_currentTarget != null)
        {
            Move();
        }
    }

    public override void Exit()
    {
        base.Exit();
        _rigidbody2D.velocity = Vector2.zero;
    }

    public void SelectTarget(ITarget currentTarget)
    {
        if (currentTarget is MonoBehaviour target)
            _currentTarget = target.transform;
    }

    private void Move()
    {
        Vector2 direction = (_currentTarget.transform.position - transform.position).normalized;
        _rigidbody2D.velocity = direction * _speed;
    }
}

