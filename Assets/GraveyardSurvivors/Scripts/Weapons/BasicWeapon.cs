using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BasicWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    [SerializeField] private float _damage;
    [SerializeField] private float _cooldown;
    [SerializeField] private float _damagePerUpgrade = 1;
    
    private CancellationTokenSource _cts;
    
    public event Action<IAttacker, IWeapon> AttackerDetected;

    public float Damage => _damage;
    public bool IsAttacking { get; private set; }
    
    private void OnEnable()
    {
        IsAttacking = false;
        
        _attackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
    }
    
    public void Upgrade()
    {
        _damage += _damagePerUpgrade;
    }
    
    public void Attack()
    {
        if (IsAttacking)
            return;
        
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);

        AttackTask().Forget();
    }
    
    public void StopAttacking() => _cts?.Cancel();

    private async UniTaskVoid AttackTask()
    {
        while (_cts?.IsCancellationRequested == false)
        {
            IsAttacking = true;
            
            _attackStrategy.Execute();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_cooldown), cancellationToken: _cts.Token);
            
            IsAttacking = false;
            
            _cts.Cancel();
        }
    }
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker == null)
            throw new Exception();
        
        AttackerDetected?.Invoke(attacker, this);
    }
}
