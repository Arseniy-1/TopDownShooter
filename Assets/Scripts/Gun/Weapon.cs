using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IInteractable
{
    [SerializeField] protected Transform ShootPoint;
    [SerializeField] private float _reloadTime;

    [SerializeField] protected Animator GunAnimator;

    private float _currentTime = 0;

    public Transform Transform { get; private set; }
    public bool IsReloaded{ get; protected set; } = false;

    private void FixedUpdate()
    {
        if (_currentTime < _reloadTime && IsReloaded == false)
            _currentTime += Time.deltaTime;

        if(_currentTime >= _reloadTime)
            Reload();
    }

    private void Awake()
    {
        Transform = transform;
    }

    public abstract void Shoot();

    public virtual void Reload()
    {
        _currentTime = 0;
        IsReloaded = true;
        //todo: Play reload animation
    }
}
