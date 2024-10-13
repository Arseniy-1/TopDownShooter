using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(Attacker))]
//rename States "MoveState"
public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();
    private Mover _moveState;

    [field: SerializeField] public State CurrentState { get; private set; }

    private void Awake()
    {
        _moveState = GetComponent<Mover>();
        _states.Add(typeof(Mover), GetComponent<Mover>());
        _states.Add(typeof(Idle), GetComponent<Idle>());
        _states.Add(typeof(Attacker), GetComponent<Attacker>());
    }

    private void Start()
    {
        CurrentState.Enter();
    }

    private void Update()
    {
        CurrentState.OnUpdate();
    }

    public void StartIdle()
    {
        ChangeState(typeof(Idle));
    }

    public void StartMove(ITarget target)
    {
        _moveState.SelectTarget(target);
        ChangeState(typeof(Mover));
    }

    public void StartAttack()
    {
        ChangeState(typeof(Attacker));
    }

    private void ChangeState(Type type)
    {
        CurrentState.Exit();
        CurrentState = _states[type];
        CurrentState.Enter();
    }
}
