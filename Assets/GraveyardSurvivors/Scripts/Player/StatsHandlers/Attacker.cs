using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacker : AttackerHandlerBase<CharacterStats>
{
    [SerializeField] private float _cooldown = 1.5f;

    private float _attackSpeed;
    private float _attackRadius;
    private Coroutine _coroutine;
    private IWeapon _weapon;

    public override void SetWeapon(IWeapon weapon)
    {
        _weapon = weapon ?? throw new Exception();
    }

    public override void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    public override void UpdateStats(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
    }

    protected override IEnumerator AttackingCoroutine()
    {
        float currentCooldown = _cooldown;
        
        while (enabled)
        {
            if (!Mathf.Approximately(currentCooldown, _cooldown))
            {
                _cooldown = UserUtils.SubstractPercentFromNumber(_cooldown, _attackSpeed);
                
                currentCooldown = _cooldown;
            }
        
            var wait = new WaitForSeconds(currentCooldown);
            
            _weapon.Attack(currentCooldown);

            yield return wait;
        }
    }

    public override void OnEnemyDetected(IAttacker attacker)
    {
        attacker.TakeDamage(_weapon.Info.Damage);
    }
}
