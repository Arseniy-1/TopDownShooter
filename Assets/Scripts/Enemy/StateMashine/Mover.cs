using UnityEngine;

public class Mover : State
{
    private float _speed =4;
    //[SerializeField] private Flipper flipper;

    private Animator _animator;
    private string _runningTrigger = "IsRunning";
    private Rigidbody2D _rigidbody2D;
    private Flipper _flipper;
    private TargetProvider _targetProvider;

    public Mover(Flipper flipper, Rigidbody2D rigidbody2D, Animator animator, TargetProvider targetProvider)
    {
        _flipper = flipper;
        _rigidbody2D = rigidbody2D;
        _animator = animator;
        _targetProvider = targetProvider;
    }

    //public override void Initialize(Flipper flipper, Rigidbody2D rigidbody2D, Animator animator)
    //{
    //    _flipper = flipper;
    //    _rigidbody2D = rigidbody2D;
    //    _animator = animator;
    //}

    public override void Enter()
    {
        //_animator.
    }

    public override void OnUpdate()
    {
        Vector2 direction = (_targetProvider.Target.Position - _rigidbody2D.position).normalized;
        _rigidbody2D.velocity = direction * _speed;
        _flipper.CorrectFlip(_rigidbody2D.velocity.x);
    }

    public override void Exit()
    {
        _rigidbody2D.velocity = Vector2.zero;
    }
}

