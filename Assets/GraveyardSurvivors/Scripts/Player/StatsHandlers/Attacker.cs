using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
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
        _weapon.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _weapon.AttackerDetected -= OnAttackerDetected;
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
            
            _weapon.Attack();
            
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
    
    private void OnAttackerDetected(IAttacker attacker)
    {
        float damage = _weapon.Damage;

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
        _weapon.Upgrade();
    }

    public void StopAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}