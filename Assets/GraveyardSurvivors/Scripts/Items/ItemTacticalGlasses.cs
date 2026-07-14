using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTacticalGlasses : Item, IUpgradeable, IAttackItem
{
    [SerializeField] private int _damagePercentBoost = 20;
    [SerializeField] private int _healthThreshold = 90;
    [SerializeField] private int _damagePerUpgrade = 5;
    
    private IPlayer _player;
    private HashSet<Enemy> _enemies;
    
    public override string CurrentDescription => $"Deal +{_damagePercentBoost}% more damage to enemies above {_healthThreshold}% HP. \n" +
                                                 $"Add +{_damagePerUpgrade}% to damage percentage for each upgrade.";

    protected override void Awake()
    {
        base.Awake();

        _enemies = new HashSet<Enemy>();
    }

    public void SetPlayer(IPlayer player)
    {
        if (_player != null)
            throw new Exception("Player is already set");
        
        _player = player;

        _player.EnemyDetected += OnEnemyDetected;
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.EnemyDetected -= OnEnemyDetected;
        }

        if (_enemies.Count != 0)
        {
            foreach (var enemy in _enemies)
            {
                Unsubscribe(enemy);
            }
        }
        
        _enemies.Clear();
    }

    public void Upgrade() => _damagePercentBoost += _damagePerUpgrade;
    
    private void OnEnemyDetected(Enemy enemy)
    {
        if (!IsHealthAboveThreshold(enemy))
            return;
        
        if (!_enemies.Add(enemy))
            return;
        
        enemy.TookDamage += OnEnemyTookDamage;
        enemy.NoHealthLeft += OnNoHealthLeft;
        
        enemy.CurrentStats.IncomingDamageMultiplier += _damagePercentBoost;
    }

    private void OnNoHealthLeft(Enemy enemy)
    {
        if (_enemies.Contains(enemy))
        {
            Unsubscribe(enemy);

            _enemies.Remove(enemy);
        }
        else
        {
            throw new Exception(nameof(_enemies) + $" doesnt have {enemy}");
        }
    }

    private void OnEnemyTookDamage(Enemy enemy)
    {
        if (IsHealthAboveThreshold(enemy))
            return;
        
        enemy.CurrentStats.IncomingDamageMultiplier -= _damagePercentBoost;
        
        Unsubscribe(enemy);

        _enemies.Remove(enemy);
    }

    private void Unsubscribe(Enemy enemy)
    {
        enemy.TookDamage -= OnEnemyTookDamage;
        enemy.NoHealthLeft -= OnNoHealthLeft;
    }

    private bool IsHealthAboveThreshold(IAttacker enemy)
    {
        float percentage = UserUtils.CalculatePercentageOf(enemy.CurrentHealth, enemy.MaxHealth);
        
        return percentage >= _healthThreshold;
    }
}
