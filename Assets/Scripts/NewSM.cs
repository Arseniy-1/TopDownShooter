using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public bool HasTarget => _targetProvider.Target != null && _targetProvider.Target.Position != null;
        public Vector2 Position => transform.position;

        private void Awake()
        {
            _stateMachine = new StateMachineFactory(this);
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
        private Enemy _bot;

        public StateMachineFactory(Enemy bot) => _bot = bot;

        public StateMachine Create()
        {
            IdleState idleState = new IdleState(_bot);
            MoveState moveState = new MoveState(_bot);
            CollectState collectState = new CollectState(_bot);
            PutState putState = new PutState(_bot);

            IdleTransition idleTransition = new IdleTransition(idleState, _bot);
            MoveTransition moveTransition = new MoveTransition(moveState, _bot);
            CollectTransition collectTransition = new CollectTransition(collectState, _bot);
            PutTransition putTransition = new PutTransition(putState, _bot);

            idleState.AddTransition(moveTransition);

            moveState.AddTransition(collectTransition);
            moveState.AddTransition(putTransition);
            moveState.AddTransition(idleTransition);

            collectState.AddTransition(idleTransition);
            collectState.AddTransition(moveTransition);

            putState.AddTransition(idleTransition);
            putState.AddTransition(moveTransition);

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
        protected readonly Enemy Bot;

        public Transition(State nextState, Enemy bot)
        {
            _nextState = nextState;
            Bot = bot;
        }

        public bool IsOpen { get; internal set; }

        public State Transit() =>
            _nextState;

        public void Close() =>
            IsOpen = false;

        internal abstract void Update(); /*логика проверки можно ли переходить*/

        protected void Open() =>
            IsOpen = true;
    }

    public class IdleState : State
    {
        public IdleState(Enemy bot) : base(bot)
        {
        }
    }

    public class MoveState : State
    {
        private Rigidbody2D rigidbody2;

        public MoveState(Rigidbody2D rigidbody2D)
        {

        }

        protected override void Work()
        {
        }
    }

    public class CollectState : State
    {
        public CollectState(Enemy bot) : base(bot)
        {
        }

        protected override void Work() =>
            Enemy.Collect();
    }

    public class PutState : State
    {
        public PutState(Enemy bot) : base(bot)
        {
        }

        protected override void Work() =>
            Enemy.Put();
    }

    public class IdleTransition : Transition
    {
        public IdleTransition(State nextState, Enemy bot) : base(nextState, bot)
        {
        }

        internal override void Update()
        {
            if (Bot.CurrentTarget == null)
                Open();
        }
    }

    public class MoveTransition : Transition
    {
        public MoveTransition(State nextState, Enemy bot) : base(nextState, bot)
        {
        }

        internal override void Update()
        {
            if (Bot.IsNearestToTarget == false)
                Open();
        }
    }

    public class CollectTransition : Transition
    {
        public CollectTransition(State nextState, Enemy bot) : base(nextState, bot)
        {
        }

        internal override void Update()
        {
            if (Bot.IsNearestToTarget && Bot.CurrentTarget is Resource)
                Open();
        }
    }

    public class PutTransition : Transition
    {
        public PutTransition(State nextState, Enemy bot) : base(nextState, bot)
        {
        }

        internal override void Update()
        {
            if (Bot.IsNearestToTarget && Bot.CurrentTarget is Base)
                Open();
        }
    }
}