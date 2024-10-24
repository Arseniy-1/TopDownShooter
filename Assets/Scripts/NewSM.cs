using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

//public interface ITarget
//{
//    public Transform Transform { get; }
//}
namespace Assets.Strategy.StateMachines
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField] private Health _health;
        [SerializeField] private Weapon _currentWeapon;
        [SerializeField] private Hand _hand;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [SerializeField] private float _attackRange;
        [SerializeField] private float _lookingDistance;

        [SerializeField] private TargetScaner _targetScaner;
        [SerializeField] private float _scanDelay;
        [SerializeField] private Flipper _flipper;

        [SerializeField] private Animator _animator;
        private StateMachine _stateMachine;
        private TargetProvider _targetProvider;

        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
       
        public bool HasWeapon => _currentWeapon != null;
        public bool HasTarget => _targetProvider.Target != null && _targetProvider.Target.Position != null;
        public Vector2 Position => transform.position;

        private void Awake()
        {
            _stateMachine = new StateMachineFactory(this, _rigidbody2D, _flipper, _targetProvider, _currentWeapon).Create();
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        internal void Put() => throw new NotImplementedException();
        internal void Move() => throw new NotImplementedException();
        internal void Collect() => throw new NotImplementedException();
    }

    public class StateMachineFactory
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly Flipper _flipper;
        private readonly TargetProvider _targetProvider;
        private readonly Weapon _weapon;
        private readonly Enemy _enemy;

        public StateMachineFactory(Enemy enemy, Rigidbody2D rigidbody2D, Flipper flipper, TargetProvider targetProvider, Weapon weapon)
        {
            _enemy = enemy;
            _rigidbody2D = rigidbody2D;
            _flipper = flipper;
            _targetProvider = targetProvider;
            _weapon = weapon;
        }

        public StateMachine Create()
        {
            IdleState idleState = new IdleState();
            MoveState moveState = new MoveState(_rigidbody2D,_flipper,_targetProvider, _enemy.Speed);
            Attack attackState = new Attack(_weapon);

            IdleTransition idleTransition = new IdleTransition(idleState, _enemy);
            MoveTransition moveTransition = new MoveTransition(moveState, _enemy, _targetProvider);
            AttackTransition attackTransition = new AttackTransition(attackState, _enemy, _targetProvider);

            idleState.AddTransition(moveTransition);
            idleState.AddTransition(attackTransition);

            moveState.AddTransition(attackTransition);
            moveState.AddTransition(idleTransition);

            attackState.AddTransition(idleTransition);
            attackState.AddTransition(moveTransition);

            return new StateMachine(idleState);
        }
    }

    public class StateMachine
    {
        private readonly State _startState;
        private State _currentState;

        public StateMachine(State startState)
        {
            _startState = startState;
            Reset();
        }

        public void Update()
        {
            State nextState = _currentState.NextState;

            if (nextState != null)
                ChangeState(nextState);

            _currentState.Update();
        }

        private void Reset() =>
            ChangeState(_startState);

        private void ChangeState(State state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
    }

    public abstract class State
    {
        private readonly List<Transition> _transitions = new List<Transition>();

        //protected State(Bot bot) => Bot = bot;

        public State NextState
        {
            get
            {
                foreach (Transition transition in _transitions)
                    if (transition.IsOpen)
                        return transition.Transit();

                return null;
            }
        }

        public void AddTransition(Transition transition)
        {
            if (_transitions.Contains(transition) == false)
                _transitions.Add(transition);
        }

        public void Enter()
        {
            foreach (Transition transition in _transitions)
                transition.Close();
        }

        public void Exit() { }

        internal void Update()
        {
            foreach (Transition transition in _transitions)
                transition.Update();

            Work();
        }

        protected virtual void Work() { }
    }

    public abstract class Transition
    {
        private readonly State _nextState;
        protected  Enemy Enemy;

        public Transition(State nextState, Enemy bot)
        {
            _nextState = nextState;
            Enemy = bot;
        }

        public bool IsOpen { get; internal set; }

        public State Transit() =>
            _nextState;

        public void Close() =>
            IsOpen = false;

        internal abstract void Update();

        protected void Open() =>
            IsOpen = true;
    }

    public class IdleState : State
    {
        public IdleState()
        {
        }
    }

    public class MoveState : State
    {
        private Rigidbody2D _rigidbody2D;
        private Flipper _flipper;
        private TargetProvider _targetProvider;
        private float _speed;

        public MoveState(Rigidbody2D rigidbody2D, Flipper flipper, TargetProvider targetProvider, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _flipper = flipper;
            _targetProvider = targetProvider;
            _speed = speed;
        }

        protected override void Work()
        {
            Vector2 direction = (_targetProvider.Target.Position - _rigidbody2D.position).normalized;
            _rigidbody2D.velocity = direction * _speed;
            _flipper.CorrectFlip(_rigidbody2D.velocity.x);
        }
    }

    public class Attack : State
    {
        private readonly Weapon _weapon;

        public Attack(Weapon weapon)
        {
            _weapon = weapon;
        }

        protected override void Work()
        {
            if (_weapon != null)
                _weapon.Shoot();
        }
    }

    public class IdleTransition : Transition
    {
        public IdleTransition(State nextState, Enemy bot) : base(nextState, bot)
        {
        }

        internal override void Update()
        {
            if (Enemy.HasTarget == false)
                Open();
        }
    }

    public class MoveTransition : Transition
    {
        private readonly TargetProvider _targetProvider;

        public MoveTransition(State nextState, Enemy enemy, TargetProvider targetProvider) : base(nextState, enemy)
        {
            _targetProvider = targetProvider;
        }

        internal override void Update()
        {
                Vector2 position = Enemy.transform.position;
                float targetDistance = (_targetProvider.Target.Position - position).magnitude;

                if(targetDistance > Enemy.AttackRange)
                     Open();
        }
    }

    public class AttackTransition : Transition
    {
        private readonly TargetProvider _targetProvider;

        public AttackTransition(State nextState, Enemy enemy, TargetProvider targetProvider) : base(nextState, bot)
        {
            _targetProvider = targetProvider;
        }

        internal override void Update()
        {
            Vector2 position = Enemy.transform.position;
            float targetDistance = (_targetProvider.Target.Position - position).magnitude;

            if (targetDistance <= Enemy.AttackRange)
                Open();
        }
    }

    public class Red
    {

    }
}