using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, ITarget, IDamagable
{
    [SerializeField] private Health _health;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Hand _hand;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private float _lookingDistance;

    [SerializeField] private TargetScaner _targetScaner;
    [SerializeField] private float _scanDelay;
    [SerializeField] private Flipper _flipper;

    [SerializeField] private Animator _animator;

    private StateMachine _stateMachine;
    private TargetProvider _targetProvider;

    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    public bool HasTarget => _targetProvider.Target != null && _targetProvider.Target.Position != null;
    public bool HasWeapon => _currentWeapon != null;
    public Vector2 Position => transform.position;


    private void Awake()
    {
        _targetProvider = new TargetProvider();
        _health = GetComponent<Health>();

        _stateMachine = new StateMachine(_flipper, _rigidbody2D, _animator, _targetProvider);
    }

    private void OnEnable()
    {
        StartCoroutine(SelectingTarget());
        _health.Died += RaiseDeath;
        EnquipWeapon(_currentWeapon);
    }

    private void OnDisable()
    {
        _health.Died -= RaiseDeath;
    }

    private void Update()
    {
        _stateMachine.OnUpdate();

        if (HasTarget)
        {
            Vector2 position = transform.position;
            float targetDistance = (_targetProvider.Target.Position - position).magnitude;

            if (targetDistance <= AttackRange)
            {
                _hand.SpotTarget(_targetProvider.Target.Position);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }

    private void Attack()
    {
        if (_currentWeapon != null)
        {
            Attack();
            _currentWeapon.Shoot();
        }
    }

    private void Stay()
    {
        _stateMachine.StartIdle();
    }

    private void Follow()
    {
        _stateMachine.StartMove();
    }

    private void EnquipWeapon(Weapon weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.Transform.parent = _hand.transform;
        _currentWeapon.Transform.position = _hand.transform.position;
        _currentWeapon.Transform.rotation = _hand.transform.rotation;
    }

    private IEnumerator SelectingTarget()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanDelay);

        while (enabled)
        {
            yield return delay;

            List<ITarget> targets = _targetScaner.Scan();

            if (targets.Count > 0)
            {
                Vector2 position = transform.position;
                List<ITarget> sortedTargets = targets.OrderBy(target => (target.Position - position).magnitude).ToList();
                _targetProvider.SetTarget(sortedTargets[0]);
            }
        }
    }

    private void RaiseDeath()
    {
        Destroy(gameObject);
    }
}
