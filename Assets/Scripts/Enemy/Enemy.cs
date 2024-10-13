using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, ITarget, IDamagable
{
    [SerializeField] private StateMachine _stateMachine;

    [SerializeField] private Health _health;
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private Hand _hand;

    [SerializeField] private float _attackRange;
    [SerializeField] private float _lookingDistance;

    [SerializeField] private TargetScaner _targetScaner;
    [SerializeField] private float _scanDelay;
    [SerializeField] private Flipper _flipper;

    private ITarget _currentTarget;

    public bool HasTarget => _currentTarget != null && _currentTarget.Transform != null;
    public Transform Transform { get; private set; }

    private void Awake()
    {
        Transform = transform;
        _health = GetComponent<Health>();
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
        if (HasTarget)
        {
            float targetDistance = (_currentTarget.Transform.position - transform.position).magnitude;

            if (targetDistance <= _lookingDistance)
            {
                Follow();
            }

            if(targetDistance <= _attackRange)
            {
                _hand.SpotTarget(_currentTarget.Transform.position);

                if (_currentWeapon != null)
                {
                    Attack();
                    _currentWeapon.Shoot();
                }
            }




            if (_stateMachine.CurrentState is Attacker == false)
            {
                _stateMachine.StartMove(_currentTarget);
            }

            if (targetDistance <= _attackRange)
            {
                _hand.SpotTarget(_currentTarget.Transform.position);

                if (_currentWeapon != null)
                {
                    _stateMachine.StartAttack();
                    _currentWeapon.Shoot();
                }
            }
            else
            {
                _stateMachine.StartMove(_currentTarget);
            }
        }
        else
        {
            _stateMachine.StartIdle();
        }
    }

    public void TakeDamage(float amount)
    {
        _health.TakeDamage(amount);
    }

    private void Attack()
    {
        _stateMachine.StartIdle();
    }

    private void Stay()
    {
        _stateMachine.StartIdle();
    }

    private void Follow()
    {
        _stateMachine.StartMove(_currentTarget);
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

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            PaymentSystemsFactory paymentSystemFactory = new PaymentSystemsFactory();

            OrderForm orderForm = new OrderForm();
            PaymentHandler paymentHandler = new PaymentHandler();

            string orderFormInput = orderForm.ShowForm(paymentSystemFactory.GetPaymentSystemNames());
            IFactory currentPaymentFactory = paymentSystemFactory.Create(orderFormInput);
            IPaymentSystem paymentSystem = currentPaymentFactory.Create();

            paymentHandler.ShowPaymentResult(paymentSystem);
        }
    }

    public class OrderForm
    {
        public string ShowForm(List<string> paymentSystemsNames)
        {
            string userInput;

            Console.Write("Мы принимаем: ");
            Console.WriteLine(string.Join(", ", paymentSystemsNames));

            Console.WriteLine("Какое системой вы хотите совершить оплату?");
            userInput = Console.ReadLine();

            return userInput;
        }
    }

    public class PaymentHandler
    {
        public void ShowPaymentResult(IPaymentSystem paymentSystem)
        {
            if (paymentSystem == null)
                throw new InvalidOperationException(nameof(paymentSystem));

            Console.WriteLine($"Вы оплатили с помощью {paymentSystem.Name}");

            Console.WriteLine($"Проверка платежа через {paymentSystem.Name}...");

            Console.WriteLine("Оплата прошла успешно!");
        }
    }

    public class PaymentSystemsFactory
    {
        private Dictionary<string, IFactory> _factorys;

        private string _qiwiID = "QIWI";
        private string _webMoneyID = "WebMoney";
        private string _cardID = "Card";
        public PaymentSystemsFactory()
        {
            _factorys = new Dictionary<string, IFactory>
            {
                {_qiwiID, new QiwiPaymentSystemFactory(_qiwiID) },
                {_webMoneyID, new QiwiPaymentSystemFactory(_webMoneyID) },
                {_cardID, new QiwiPaymentSystemFactory(_cardID) }
            };
        }

        public List<string> GetPaymentSystemNames()
        {
            List<string> result = new List<string>();

            foreach (string name in _factorys.Keys)
                result.Add(name);

            return result;
        }

        public IFactory Create(string ID)
        {
            if (_factorys.ContainsKey(ID) == false)
                throw new InvalidOperationException(nameof(ID));

            return _factorys[ID];
        }
    }

    public class QiwiPaymentSystemFactory : IFactory
    {
        private string _ID;

        public QiwiPaymentSystemFactory(string ID)
        {
            _ID = ID ?? throw new InvalidOperationException(nameof(ID));
        }

        public IPaymentSystem Create() => new Qiwi(_ID);
    }

    public class Qiwi : IPaymentSystem
    {
        public Qiwi(string name)
        {
            Name = name ?? throw new InvalidOperationException(nameof(name));
        }

        public string Name { get; private set; }

        public void ShowLoadingScreen()
        {
            Console.WriteLine("Перевод на страницу QIWI...");
        }
    }

    public class CardPaymentSystemFactory : IFactory
    {
        private string _ID;

        public CardPaymentSystemFactory(string ID)
        {
            _ID = ID ?? throw new InvalidOperationException(nameof(ID));
        }

        public IPaymentSystem Create() => new Card(_ID);
    }

    public class Card : IPaymentSystem
    {
        public Card(string name)
        {
            Name = name ?? throw new InvalidOperationException(nameof(name));
        }

        public string Name { get; private set; }

        public void ShowLoadingScreen()
        {
            Console.WriteLine("Вызов API банка эмитера карты Card...");
        }
    }

    public class WebMoneyPaymentSystemFactory : IFactory
    {
        private string _ID;

        public WebMoneyPaymentSystemFactory(string ID)
        {
            _ID = ID ?? throw new InvalidOperationException(nameof(ID));
        }

        public IPaymentSystem Create() => new WebMoney(_ID);
    }

    public class WebMoney : IPaymentSystem
    {
        public WebMoney(string name)
        {
            Name = name ?? throw new InvalidOperationException(nameof(name));
        }

        public string Name { get; private set; }

        public void ShowLoadingScreen()
        {
            Console.WriteLine("Вызов API WebMoney...");
        }
    }

    public interface IFactory
    {
        IPaymentSystem Create();
    }

    public interface IPaymentSystem
    {
        public string Name { get; }

        void ShowLoadingScreen();
    }
}

