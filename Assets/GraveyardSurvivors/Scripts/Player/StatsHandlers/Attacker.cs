using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;
    [SerializeField] private float _cooldown = 1.5f;
    
    private Coroutine _coroutine;
    private IAttacker _attacker;
    
    public bool IsAttacking {get; private set;}

    public void Init(IAttacker attacker)
    {
        _attacker = attacker;
    }
    
    private void OnEnable()
    {
        foreach (var weapon in _weapons)
        {
            weapon.AttackerDetected += OnAttackerDetected;
        }
    }

    private void OnDisable()
    {
        foreach (var weapon in _weapons)
        {
            weapon.AttackerDetected -= OnAttackerDetected;
        }
    }

    public void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    private IEnumerator AttackingCoroutine()
    {
        IsAttacking = true;
        
        float currentCooldown = _cooldown;
        
        while (enabled)
        {
            currentCooldown = CompareCooldown(currentCooldown);
            
            var wait = new WaitForSecondsRealtime(currentCooldown);
            
            yield return wait;

            foreach (var weapon in _weapons)
            {
                weapon.Attack();
            }
            
            IsAttacking = false;
        }
    }

    private float CompareCooldown(float currentCooldown)
    {
        if (!Mathf.Approximately(currentCooldown, _cooldown))
        {
            _cooldown = _cooldown.SubtractPercentFromNumber(_attacker.Speed);
            
            currentCooldown = _cooldown;
        }

        return currentCooldown;
    }
    
    private void OnAttackerDetected(IAttacker attacker, Weapon weapon)
    {
        float damage = weapon.Damage;

        float currentCritChance = _attacker.CritChance.AddPercentToNumber(_attacker.Luck);

        if (IsEnoughChanceToCrit(currentCritChance))
        {
            damage *= _attacker.CritMultiplier;
        }
        
        attacker.TakeDamage(damage);
    }

    private bool IsEnoughChanceToCrit(float critChance)
    {
        if (critChance >= UserUtils.s_HighestPercent)
        {
            return true;
        }
        
        float randomNumber = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);
        
        return critChance >= randomNumber;
    }

    public void UpgradeWeapons()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Upgrade();
        }
    }

    public void StopAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void Reset()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Reset();
        }
    }
}