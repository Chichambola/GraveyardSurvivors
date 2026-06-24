using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Sherbert.Framework.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weaponsPrefab;
    
    private Coroutine _coroutine;
    private IAttacker _attacker;
    private List<Weapon> _currentWeapons;

    public void Init(IAttacker attacker)
    {
        _attacker = attacker;
    }

    private void Awake()
    {
        _currentWeapons = new List<Weapon>();
    }

    private void OnValidate()
    {
        var hashSet = new HashSet<Weapon>();
        
        _weaponsPrefab.RemoveAll(weapon => hashSet.Add(weapon) == false);
    }

    private void OnEnable()
    {
        foreach (var prefab in _weaponsPrefab)
        {
            var weapon = CreateWeapon(prefab);

            _currentWeapons.Add(weapon);
        }
    }

    private void OnDisable()
    {
        foreach (var weapon in _currentWeapons)
        {
            weapon.AttackerDetected -= OnAttackerDetected;
        }
    }
    
    public void UpgradeWeapon(Weapon upgrade)
    {
        var type = upgrade.GetType();
        
        _currentWeapons.FirstOrDefault(w => w.GetType() == type)?.Upgrade();
    }

    public void AddWeapon(Weapon weapon)
    {
        weapon = CreateWeapon(weapon);
        _currentWeapons.Add(weapon);
    }

    public bool HasWeapon(Weapon weapon)
    {
        var type = weapon.GetType();

        var tempWeapon = _currentWeapons.FirstOrDefault(w => w.GetType() == type);
        
        return tempWeapon != null;
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
    
    private Weapon CreateWeapon(Weapon prefab)
    {
        var weapon = Instantiate(prefab, transform.position, Quaternion.LookRotation(transform.forward), transform);
            
        weapon.AttackerDetected += OnAttackerDetected;
            
        weapon.StartAttacking();
        return weapon;
    }
}