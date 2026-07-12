using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Sherbert.Framework.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<Weapon> _weaponsPrefab;
    [SerializeField]private List<Effect> _damageEffects;

    public event Action<Enemy> EnemyDetected;
    
    private Coroutine _coroutine;
    private List<Weapon> _currentWeapons;
    private float _currentAttackSpeed;

    private void Awake()
    {
        _currentWeapons = new List<Weapon>();
        _damageEffects = new List<Effect>();
    }

    private void OnValidate()
    {
        var hashSet = new HashSet<Weapon>();
        
        _weaponsPrefab.RemoveAll(weapon => hashSet.Add(weapon) == false);
    }

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;

        _currentAttackSpeed = 0;
        
        foreach (var prefab in _weaponsPrefab)
        {
            var weapon = CreateWeapon(prefab);

            _currentWeapons.Add(weapon);
        }
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
        
        foreach (var weapon in _currentWeapons)
        {
            weapon.AttackerDetected -= OnAttackerDetected;
        }
        
        _currentWeapons.Clear();
        
        _damageEffects.Clear();
    }

    public void ProcessWeapon(Weapon weapon)
    {
        if (HasWeapon(weapon))
        {
            UpgradeWeapon(weapon);
        }
        else
        {
            AddWeapon(weapon);
        }
    }

    public bool HasWeapon(Weapon weapon)
    {
        var type = weapon.GetType();

        var tempWeapon = _currentWeapons.FirstOrDefault(w => w.GetType() == type);
        
        return tempWeapon != null;
    }

    public void AddEffect(Effect effect)
    {
        _damageEffects.Add(effect);
    }
    
    private void OnAttackerDetected(IAttacker attacker, Weapon weapon)
    {
        if (attacker is Enemy enemy)
        {
            EnemyDetected?.Invoke(enemy);
        }
        
        if (_damageEffects.Count != 0)
        {
            foreach (var effect in _damageEffects)
            {
                effect.Execute(attacker);
            }
        }
        
        float damage = weapon.Damage;

        float currentCritChance = _player.CritChance.AddPercentToNumber(_player.Luck);

        if (IsEnoughChanceToCrit(currentCritChance))
        {
            damage *= _player.CritMultiplier;
        }
        
        attacker.TakeDamage(damage);
    }

    private bool IsEnoughChanceToCrit(float critChance)
    {
        if (critChance >= UserUtils.s_highestPercent)
        {
            return true;
        }
        
        float randomNumber = Random.Range(UserUtils.s_lowestPercent, UserUtils.s_highestPercent);
        
        return critChance >= randomNumber;
    }
    
    private Weapon CreateWeapon(Weapon prefab)
    {
        var weapon = Instantiate(prefab, transform.position, Quaternion.LookRotation(transform.forward), transform);
            
        weapon.AttackerDetected += OnAttackerDetected;
            
        weapon.StartAttacking();
        
        SetWeaponCooldown(weapon, _currentAttackSpeed);
        
        return weapon;
    }
    
    private void OnStatsChanged(CharacterStats stats)
    {
        if (Mathf.Approximately(stats.AttackSpeed, _currentAttackSpeed))
            return;
        
        float attackSpeed = stats.AttackSpeed - _currentAttackSpeed;
        
        _currentAttackSpeed = stats.AttackSpeed;

        foreach (var weapon in _currentWeapons)
        {
            SetWeaponCooldown(weapon, attackSpeed);
        }
    }

    private void SetWeaponCooldown(Weapon weapon, float attackSpeed)
    {
        float cooldown = weapon.CurrentCooldown;

        cooldown = cooldown.SubtractPercentFromNumber(attackSpeed);
            
        weapon.SetCooldown(cooldown);
    }
    
    private void UpgradeWeapon(Weapon upgrade)
    {
        var type = upgrade.GetType();
        
        _currentWeapons.FirstOrDefault(w => w.GetType() == type)?.Upgrade();
    }

    private void AddWeapon(Weapon weapon)
    {
        weapon = CreateWeapon(weapon);
        
        _currentWeapons.Add(weapon);
    }
}