using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;

public class WeaponBow : Weapon
{
    [SerializeField] private RangeAttackStrategy _attackStrategy;
    [SerializeField] private ProjectileSpawner _arrowSpawner;

    public override event Action<IAttacker, IWeapon> AttackerDetected;

    private Coroutine _attackingRoutine;
    private WaitForSeconds _attackTime;
    
    public override string BaseDescription => $"Range weapon.\n" +
                                              $"Shoots {_attackStrategy.InitialProjectileAmount} at a time prioritizing enemies in front of the player.";
    public override string UpgradeDescription => $"Add +{BonusDamagePerUpgrade} damage \n" +
                                                 $"Add +{_attackStrategy.ProjectilePerUpgrade} to  total projectile amount.";
    
    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        _arrowSpawner.ProjectileHitEnemy += OnProjectileReleased;
        
        _attackTime = new WaitForSeconds(Cooldown);
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
        _arrowSpawner.ProjectileHitEnemy -= OnProjectileReleased;
    }
    
    
    public override void Attack()
    {
        _attackStrategy.Execute();
    }

    public override void Upgrade()
    {
        _attackStrategy.Upgrade();
        
        base.Upgrade();
    }

    public override void StartAttacking()
    {
        if (_attackingRoutine != null)
            StopCoroutine(_attackingRoutine);

        _attackingRoutine = StartCoroutine(AttackRoutine());
    }

    public override void SetCooldown(float cooldown)
    {
        base.SetCooldown(cooldown);
        
        _attackTime = new WaitForSeconds(cooldown);
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            _arrowSpawner.Spawn(enemy);  
        }
    }

    private IEnumerator AttackRoutine()
    {
        IsAttacking = true;
        
        while (enabled)
        {
            yield return _attackTime;
            
            _attackStrategy.Execute();
            
            IsAttacking = false;
        }
    }
    
    private void OnProjectileReleased(IAttacker attacker)
    {
        AttackerDetected?.Invoke(attacker, this);
    }
}
