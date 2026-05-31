using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _cooldown = 1.5f;
    
    private float _attackSpeed;
    private float _attackRadius;
    private float _critChance;
    private float _critMultipler;
    private Coroutine _coroutine;
    private float _damageMultiplier;

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
        _weapon.AttackerDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
        _weapon.AttackerDetected -= OnEnemyDetected;
    }

    public void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    private IEnumerator AttackingCoroutine()
    {
        float currentCooldown = _cooldown;
        
        while (enabled)
        {
            currentCooldown = CompareCooldown(currentCooldown);
        
            var wait = new WaitForSeconds(currentCooldown);
            
            _weapon.Attack(_attackRadius);

            yield return wait;
        }
    }

    private float CompareCooldown(float currentCooldown)
    {
        if (!Mathf.Approximately(currentCooldown, _cooldown))
        {
            _cooldown = _cooldown.SubtractPercentFromNumber(_attackSpeed);
            
            currentCooldown = _cooldown;
        }

        return currentCooldown;
    }
    
    private void OnStatsChanged(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
        _damageMultiplier = stats.DamageAmplifier;
        _critChance = stats.CritChance;
        _critMultipler = stats.CritMultiplier;
    }
    
    private void OnEnemyDetected(IAttacker attacker)
    {
        if (attacker is Enemy enemy)
        {
            float damage = _weapon.Damage.AddPercentToNumber(_damageMultiplier);

            float currentCritChance = _critChance.AddPercentToNumber(_player.CurrentStats.Luck);

            if (IsEnoughChanceToCrit(currentCritChance))
            {
                damage *= _critMultipler;
            }
        
            enemy.TakeDamage(damage);
        }
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
}
