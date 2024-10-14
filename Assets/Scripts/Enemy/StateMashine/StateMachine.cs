using UnityEngine;
using System.Collections.Generic;
using System;

public class StateMachine
{
    private Dictionary<Type, State> _states = new Dictionary<Type, State>();

    [field: SerializeField] public State CurrentState { get; private set; }
    
    public StateMachine(Flipper flipper, Rigidbody2D rigidbody2D, Animator animator, TargetProvider targetProvider)
    {
        _states.Add(typeof(Mover), new Mover(flipper,rigidbody2D,animator, targetProvider));
        _states.Add(typeof(Idle), new Idle(flipper, rigidbody2D, animator));
        _states.Add(typeof(Attacker), new Attacker(flipper, animator, targetProvider));

        CurrentState = _states[typeof(Idle)];
        CurrentState.Enter(); 
    }

    public void OnUpdate()
    {
        CurrentState.OnUpdate();
    }

    public void StartIdle()
    {
        ChangeState(typeof(Idle));
    }

    public void StartMove()
    {
        ChangeState(typeof(Mover));
    }

    public void StartAttack()
    {
        ChangeState(typeof(Attacker));
    }

    private void ChangeState(Type type)
    {
        if (CurrentState == _states[type])
            return;

        CurrentState.Exit();
        CurrentState = _states[type];
        CurrentState.Enter();
    }
}
