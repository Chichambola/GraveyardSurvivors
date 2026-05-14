using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(InteractionHandler), typeof(Rigidbody))]
[RequireComponent(typeof(InputReader))]
public class Player : CharacterBase, IBuffable, IAttacker, IPlayer, ILightCarrier
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private CollisionHandler _collisionHandler;
    [SerializeField] private PickablesDetector _pickUpsDetector;
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private Attacker _attacker;
    
    [Header("Stats")]
    [SerializeField] private PlayerInfo _baseStats;
    [SerializeField] private StatsViewer _statsViewer;
    [SerializeField] private float _invincibilityAfterDamage = .30f;
    
    [Header("Services")] 
    [SerializeField] private Health _health;
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ExperienceHandler _xpHandler;
    [SerializeField] private LanternLight _light;
    
    public event Action InteractionButtonPressed;
    public event Action<CharacterStats> StatsChanged;

    private int _lanternsCount;
    private bool _isInLantern;
    private bool _canTakeDamage;
    private Rigidbody _rigidbody;
    private IntervalTimer _timer;
    
    public CharacterStats CurrentStats { get; private set; }
    public Vector3 CurrentPosition => transform.position;
    public Transform Transform => transform;
    public float MoneyAmount => _wallet.CurrentMoneyAmount;
    public float CurrentHealth => CurrentStats.Health;
    public float Luck => CurrentStats.Luck;
    public bool IsAlive => CurrentStats.Health > 0;
    public bool IsLightActive => _light.IsActive;


    public float MaxHealth => CurrentStats.MaxHealth;

    public event Action<Player> CanBeReleased;

    protected override void Awake()
    {
        Animator = GetComponent<Animator>();
        Rotator = GetComponent<Rotator>();
        Mover = GetComponent<Mover>();
        Collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _interactionHandler = GetComponent<InteractionHandler>();
        _inputReader = GetComponent<InputReader>();

        InitializeStateMachine();
    }

    private void OnEnable()
    {
        if (_baseStats == null)
            throw new Exception();

        CurrentStats = _baseStats.GetStats();
        
        _pickUpsDetector.BuffDetected += AddBuff;
        _pickUpsDetector.CoinDetected += _wallet.ReceiveMoney;
        _pickUpsDetector.CrystalDetected += _xpHandler.GainExperience;
        _collisionHandler.EnemyDetected += TakeDamage;
        
        _attacker.StartAttacking();
    }

    private void OnDisable()
    {
        _pickUpsDetector.BuffDetected -= AddBuff;
        _pickUpsDetector.CoinDetected -= _wallet.ReceiveMoney;
        _collisionHandler.EnemyDetected -= TakeDamage;
        _pickUpsDetector.CrystalDetected -= _xpHandler.GainExperience;
    }

    private void Start()
    {
        _canTakeDamage = true;
        
        _light.Init();
        
        StatsChanged?.Invoke(CurrentStats);
    }

    protected override void Update()
    {
        StateMachine.Update();
        
        if (_inputReader.IsInteractionButtonPressed)
        {
            InteractionButtonPressed?.Invoke();
        }
    }

    protected override void FixedUpdate()
    {
        StateMachine.FixedUpdate();
        
        HandleMovement();
    }

    public override void HandleMovement()
    {
        Mover.MoveDyDirection(_inputReader.MovementDirection.normalized, CurrentStats.MovementSpeed);
        
        if (_inputReader.MovementDirection != Vector3.zero)
        {
            Rotator.Rotate(_inputReader.MovementDirection.normalized);
        }
    }

    public void ResetCharacteristics() { }

    public override void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void ReduceMoney(float value) => _wallet.ReduceMoney(value);

    public void ReceiveMoney(float value) => _wallet.ReceiveMoney(value);
    
    public void ChangeSpeed(float speedPercent, bool isSlowing)
    {
        if (isSlowing)
        {
            CurrentStats.MovementSpeed = CurrentStats.MovementSpeed.SubtractPercentFromNumber(speedPercent);
        }
        else
        {
            CurrentStats.MovementSpeed = CurrentStats.MovementSpeed.AddPercentToNumber(speedPercent);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_canTakeDamage)
            return;
        
        if (_evader.CanEvade(CurrentStats.EvasionChance, CurrentStats.Luck))
        {
            Debug.Log("Evaded");
        }
        else
        {
            damage = DetermineDamageAmount(damage);
        
            _canTakeDamage = false;
            
            CurrentStats.Health -= damage;
        
            _statsViewer.UpdateStats(CurrentStats);
            
            _timer = new IntervalTimer(_invincibilityAfterDamage);
            _timer.Stopped += OnDamageTimerStopped;
            _timer.Start();
        }
    }
    
    public void ApplyEffect(IEffect<IAttacker> effectFactory) { }

    public void AddBuff(IBuff buff)
    {
        CurrentStats = buff.ApplyBuff(CurrentStats);
        
        StatsChanged?.Invoke(CurrentStats);
    }

    public void RemoveBuff(IBuff buff)
    {
        CurrentStats = buff.RemoveBuff(CurrentStats);
        
        StatsChanged?.Invoke(CurrentStats);
    }
    
    public void Heal(float value)
    {
        CurrentStats.Health += value;

        if (CurrentStats.Health >= MaxHealth)
        {
            CurrentStats.Health = MaxHealth;
        }
        
        _statsViewer.UpdateStats(CurrentStats);
    }

    public void ResetRadius()
    {
        _light.ResetRadius();
        _light.SetRate(0);
    }

    public void StartLight()
    {
        _light.StartRadiusRoutine();
        _light.ResetRate();
    }

    protected override void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        
        DefineAtTransition(idleState, runState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude > 0));
        DefineAtTransition(runState, idleState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude <= 0));
        
        StateMachine.SetState(idleState);
    }
    
    private float DetermineDamageAmount(float damage)
    {
        if (_defender.CanBlock(CurrentStats.BlockChance, CurrentStats.Luck))
        {
            Debug.Log("Blocked");
            
            damage = _defender.GetBlockedDamage(damage);
        }
        
        damage = damage.AddPercentToNumber(CurrentStats.IncomingDamageMultiplier);
        
        damage = _defender.GetDamageAmount(CurrentStats.Armor, damage);
        
        return damage;
    }

    private void OnDamageTimerStopped()
    {
        _canTakeDamage = true;
        
        _timer.Stopped -= OnDamageTimerStopped;
        
        _timer?.Stop();
    }
}