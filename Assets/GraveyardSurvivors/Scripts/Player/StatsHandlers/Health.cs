using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _healCooldown;
    [SerializeField] private float _reduceDamageTime = 1f;
    [Header("Services")] 
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private StatsViewer _statsViewer;
    
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
        _isTakingLessDamage = false;
        _isOnCooldown = false;
        
        HealingTask().Forget();
    }

    public void UpdateStats() => _statsViewer.UpdateStats(_player.CurrentHealth, _player.CurrentStats.MaxHealth);

    private async UniTask ReduceDamageTask()
    {
        while (!_ctsDamage.IsCancellationRequested)
        {
            _isTakingLessDamage = true;

            await UniTask.Delay(TimeSpan.FromSeconds(_reduceDamageTime), cancellationToken: _ctsDamage.Token);
            
            _isOnCooldown = true;
            _isTakingLessDamage = false;

            await UniTask.Delay(TimeSpan.FromSeconds(_reduceDamageTime), cancellationToken: _ctsDamage.Token);

            _isOnCooldown = false;
            
            _ctsDamage.Cancel();
        }
    }
    
    private async UniTaskVoid HealingTask()
    {
        while (!_ctsHealing.Token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_healCooldown), DelayType.UnscaledDeltaTime, cancellationToken: _ctsHealing.Token);

            _player.Heal(_player.CurrentStats.HealthRegeneration);

            UpdateStats();
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
}