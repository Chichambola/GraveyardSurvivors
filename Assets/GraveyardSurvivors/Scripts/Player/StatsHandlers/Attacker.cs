using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private float _cooldown = 1.5f;

    public event Action<Enemy> EnemyWasKilled;
    
    private float _attackSpeed;
    private float _attackRadius;
    private float _critChance;
    private float _critMultipler;
    private Coroutine _coroutine;

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
            
            _weapon.Attack(currentCooldown, _attackRadius);

            yield return wait;
        }
    }

    private float CompareCooldown(float currentCooldown)
    {
        if (!Mathf.Approximately(currentCooldown, _cooldown))
        {
            _cooldown = UserUtils.SubtractPercentFromNumber(_cooldown, _attackSpeed);
            
            currentCooldown = _cooldown;
        }

        return currentCooldown;
    }
    
    private void OnStatsChanged(CharacterStats stats)
    {
        _attackSpeed = stats.AttackSpeed;
        _attackRadius = stats.AttackRadius;
        _critChance = stats.CritChance;
        _critMultipler = stats.CritMultiplier;
    }
    
    private void OnEnemyDetected(IAttacker attacker)
    {
        if (attacker != null && attacker is Enemy enemy)
        {
            float damage = _weapon.Info.Damage;

            float currentCritChance = _critChance + _player.CurrentStats.Luck;

            if (IsEnoughChanceToCrit(currentCritChance))
            {
                damage *= _critMultipler;
            }
        
            enemy.TakeDamage(damage);

            if (enemy.CurrentStats.Health <= 0)
            {
                EnemyWasKilled?.Invoke(enemy);
            }
        }
    }

    private bool IsEnoughChanceToCrit(float critChance)
    {
        float randomNumber = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);
        
        return critChance >= randomNumber;
    }
}
