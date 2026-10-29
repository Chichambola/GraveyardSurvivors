using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _healCooldown;
    [SerializeField] private float _reduceDamageTime = 1f;
    [SerializeField] private float _reduceDamageCooldown = 2f;
    [Header("Services")] 
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private SliderHandler _slider;
    
    private CancellationTokenSource _ctsHealing;
    private CancellationTokenSource _ctsDamage;
    private int _damageReduceAfterDamage = 70;
    private bool _isTakingLessDamage;
    private bool _isOnCooldown;

    private void Awake()
    {
        _ctsHealing = new CancellationTokenSource();
        _ctsHealing.RegisterRaiseCancelOnDestroy(gameObject);
    }

    private void OnEnable()
    {
        _player.StatsChanged += UpdateStats;
        
        _isTakingLessDamage = false;
        _isOnCooldown = false;
        
        HealingTask().Forget();
    }

    private void OnDisable()
    {
        _player.StatsChanged -= UpdateStats;
    }

    private void UpdateStats(CharacterStats stats)
    {
        _slider.Set(_player.CurrentHealth, stats.MaxHealth);
        
        UpdateVisuals();
    }
    
    private async UniTask ReduceDamageTask()
    {
        while (!_ctsDamage.IsCancellationRequested)
        {
            _isTakingLessDamage = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_reduceDamageTime), cancellationToken: _ctsDamage.Token);
            
            _isOnCooldown = true;
            _isTakingLessDamage = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_reduceDamageCooldown), cancellationToken: _ctsDamage.Token);

            _isOnCooldown = false;
            
            _ctsDamage.Cancel();
        }
    }
    
    private async UniTaskVoid HealingTask()
    {
        while (!_ctsHealing.Token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_healCooldown), DelayType.DeltaTime, cancellationToken: _ctsHealing.Token);
            
            _player.Heal(_player.CurrentStats.HealthRegeneration);

            UpdateVisuals();
        }
    }

    public bool TryTakeDamage(ref float damage)
    {
        if (_evader.CanEvade(_player.CurrentStats.EvasionChance, _player.CurrentStats.Luck))
        {
            Debug.Log("Evaded");

            return false;
        }

        if (_defender.TryBlockDamage(_player.CurrentStats.BlockChance, _player.CurrentStats.Luck, ref damage))
        {
            Debug.Log("Blocked");
        }

        damage = damage.AddPercentToNumber(_player.CurrentStats.IncomingDamageMultiplier);

        if (_isTakingLessDamage)
            damage = damage.SubtractPercentFromNumber(_damageReduceAfterDamage);
        
        if (_isOnCooldown || _isTakingLessDamage) 
            return true;
        
        _ctsDamage = new CancellationTokenSource();
        _ctsDamage.RegisterRaiseCancelOnDestroy(gameObject);

        ReduceDamageTask().Forget();
        
        return true;
    }

    private void UpdateVisuals()
    {
        float currentValue = _player.CurrentHealth;
        float maxHealth = _player.MaxHealth;
            
        _text.text = $"{currentValue : #.#} / {maxHealth} \n";
        
        _slider.UpdateSlider(currentValue).Forget();
    }
}