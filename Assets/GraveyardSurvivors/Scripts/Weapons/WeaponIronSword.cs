using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WeaponIronSword : Weapon
{
    [Header("Strategy")]
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    [SerializeField] private float _radiusPercentGain = 5f;
    [Header("Services")]
    [SerializeField] private IronSwordSpawner _spawner;
    [SerializeField] private KnockBack _knockBack;

    public override event Action<IAttacker, IWeapon> AttackerDetected;

    private Vector3 _debugPos;
    private WaitForSeconds _attackWait;
    private Coroutine _attackingRoutine;
    
    public override string UpgradeDescription => $"Add +{BonusDamagePerUpgrade} damage. \n" +
                                                 $"Add +{_radiusPercentGain}% to attack radius.\n" +
                                                 $"Add +{_knockBack.KnockBackPercentGain}% to knock back force;";

    public override string BaseDescription => $"Melee weapon.\n" +
                                                 $"Knocks back enemies on hit.";

    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        _spawner.ReleasedAllObjects += OnReleasedAllObjects;
        
        _attackWait = new WaitForSeconds(Cooldown);
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
        _spawner.ReleasedAllObjects -= OnReleasedAllObjects;
    }

    public override void Upgrade()
    {
        _attackStrategy.Upgrade(_radiusPercentGain);
        
        _knockBack.Upgrade();

        _spawner.Upgrade(_radiusPercentGain);
        
        base.Upgrade();
    }

    public override void Attack()
    {
        if (_attackingRoutine != null)
            StopCoroutine(_attackingRoutine);
        
        _attackingRoutine = StartCoroutine(AttackingRoutine());
    }

    public override void StopAttacking()
    {
        if (_attackingRoutine != null)
            StopCoroutine(_attackingRoutine);
    }

    public override void SetCooldown(float cooldown)
    {
        base.SetCooldown(cooldown);
        
        _attackWait = new WaitForSeconds(cooldown);
    }

    private void OnAttackerDetected(IAttacker attacker)
    {
        AttackerDetected?.Invoke(attacker, this);

        _knockBack.Apply(attacker);
    }

    private IEnumerator AttackingRoutine()
    {
        while (enabled)
        {
            _attackStrategy.SetActive(true);
            
            _attackStrategy.Execute();
            
            _spawner.Spawn(Cooldown);
            
            yield return _attackWait;
        }
    }
    
    private void OnReleasedAllObjects()
    {
        _attackStrategy.SetActive(false);
    }
}