using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(InteractionHandler), typeof(Rigidbody))]
[RequireComponent(typeof(InputReader))]
public class Player : CharacterBase, IBuffable, IAttacker, IPlayer, ILightCarrier, ITarget
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PickablesDetector _pickUpsDetector;
    [SerializeField] private InteractionHandler _interactionHandler;
    [SerializeField] private Attacker _attacker;
    
    [Header("Stats")]
    [SerializeField] private PlayerInfo _baseStats;
    [SerializeField] private StatsViewer _statsViewer;
    
    [Header("Services")] 
    [SerializeField] private Inventory _itemsHandler;
    [SerializeField] private Health _health;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private LanternLight _light;
    [SerializeField] private bool _isImmortal;
    
    public event Action InteractionButtonPressed;
    public event Action Died;
    public event Action<CharacterStats> StatsChanged;
    public event Action<float> GainedXp;
    public event Action<Item> PickedItem;
    public event Action<Enemy> EnemyDetected;
    
    private int _lanternsCount;
    private bool _isInLantern;
    private Rigidbody _rigidbody;
    private IFollower _follower;
    
    public CharacterStats CurrentStats { get; private set; }
    public Vector3 CurrentPosition => transform.position;
    public float CurrentHealth { get; private set; }
    public float MaxHealth => CurrentStats.MaxHealth;
    public float MoneyAmount => _wallet.CurrentMoneyAmount;
    public float CritChance => CurrentStats.CritChance;
    public float CritMultiplier => CurrentStats.CritMultiplier;
    public float Luck => CurrentStats.Luck;
    public bool IsLightActive => _light.IsActive;
    public bool IsAlive => CurrentHealth > 0;
    
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
        
        CurrentHealth = CurrentStats.MaxHealth;
        
        _pickUpsDetector.ItemDetected += OnItemPickedUp;
        _pickUpsDetector.CoinDetected += _wallet.ReceiveMoney;
        _pickUpsDetector.CrystalDetected += OnCrystalDetected;
        _attacker.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _pickUpsDetector.ItemDetected -= OnItemPickedUp;
        _pickUpsDetector.CoinDetected -= _wallet.ReceiveMoney;
        _pickUpsDetector.CrystalDetected -= OnCrystalDetected;
        _attacker.EnemyDetected -= OnEnemyDetected;
    }

    private void Start()
    {
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

    public override void Release() => CanBeReleased?.Invoke(this);
    
    public void ReduceMoney(float value) => _wallet.ReduceMoney(value);

    public void ReceiveMoney(float value) => _wallet.ReceiveMoney(value);
    
    public void Upgrade(CharacterStats statsToUpgrade)
    {
        CurrentStats.Upgrade(statsToUpgrade);
        StatsChanged?.Invoke(CurrentStats);
    }

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
        if (_isImmortal)
            return;
        
        if (!_health.TryTakeDamage(ref damage))
            return;
        
        CurrentHealth -= damage;
            
        _health.UpdateStats();

        if (CurrentHealth <= 0)
        {
            Died?.Invoke();
        }
    }
    
    public bool HasWeapon(Weapon weapon) => _attacker.HasWeapon(weapon);

    public void ProcessWeapon(Weapon weapon) => _attacker.ProcessWeapon(weapon);

    public void AddEffect(Effect effect) => _attacker.AddEffect(effect);

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
        CurrentHealth += value;

        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void ResetRadius(float duration) => _light.ResetRadius(duration);

    public void StartChangingRadius() => _light.StartChanging();

    public void StartLight() => _light.StartLight();
    
    public void PauseLight() => _light.PauseLight();
    
    public void AddItem(Item item) => _itemsHandler.Add(item);
    
    protected override void InitializeStateMachine()
    {
        StateMachine = new StateMachine();

        var runState = new RunState(this, Animator);
        var idleState = new IdleState(this, Animator);
        
        DefineAtTransition(idleState, runState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude > 0));
        DefineAtTransition(runState, idleState, new FuncPredicate(() => _inputReader.MovementDirection.magnitude <= 0));
        
        StateMachine.SetState(idleState);
    }

    private void OnCrystalDetected(float crystalValue) => GainedXp?.Invoke(crystalValue);
    
    private void OnEnemyDetected(Enemy enemy) => EnemyDetected?.Invoke(enemy);
    
    private void OnItemPickedUp(Item item)
    {
        _itemsHandler.Add(item);

        PickedItem?.Invoke(item);
    }
}