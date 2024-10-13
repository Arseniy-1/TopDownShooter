using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PlayerCollisionHandler))]
[RequireComponent(typeof(PlayerMover))]
[RequireComponent(typeof(Health))]

public class Player : MonoBehaviour, IDamagable, ITarget
{
    [SerializeField] private Hand _hand;
    [SerializeField] private TargetScaner _targetScaner;
    [SerializeField] private float _scanDelay;

    private Weapon _currentWeapon;
    private ITarget _currentTarget;

    private PlayerCollisionHandler _playerCollisionHandler;
    private Health _health;

    private bool _hasTarget;

    public Transform Transform {get; private set;}

    private void Awake()
    {
        Transform = transform;
        _playerCollisionHandler = GetComponent<PlayerCollisionHandler>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        StartCoroutine(SelectingTarget());
        _playerCollisionHandler.CollisionDetected += Interact;
        _health.Died += RaiseDeath;
    }

    private void OnDisable()
    {
        _playerCollisionHandler.CollisionDetected -= Interact;
        _health.Died -= RaiseDeath;
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }

    private void Update()
    {
        if (_currentTarget != null && _currentTarget.Transform != null)
        {
            _hand.SpotTarget(_currentTarget.Transform.position);

            if (_currentWeapon != null)
            {
                _currentWeapon.Shoot();
            }
        }
    }

    private void Interact(IInteractable interactable)
    {
        if (interactable is Weapon weapon)
        {
            EnquipWeapon(weapon);
        }
        //else if ()
        //{
        //}
        //else if ()
        //{
        //}
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
                List<ITarget> sortedTargets = targets.OrderBy(target => (target.Transform.position - transform.position).magnitude).ToList();
                _currentTarget = sortedTargets[0];
            }
        }
    }

    private void RaiseDeath()
    {
        Destroy(gameObject);
    }
}
