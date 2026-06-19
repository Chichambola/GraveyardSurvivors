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
    [SerializeField] private SerializableDictionary<Weapon, float> _weaponsAndCooldowns;
    
    private Coroutine _coroutine;
    private IAttacker _attacker;

    public void Init(IAttacker attacker)
    {
        _attacker = attacker;
    }

    private void OnEnable()
    {
        foreach (var prefab in _weaponsAndCooldowns.Keys)
        {
            var weapon = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            
            weapon.AttackerDetected += OnAttackerDetected;

            foreach (var cooldown in _weaponsAndCooldowns.Values)
            {
                Debug.Log($"{weapon} {cooldown}");
                
                weapon.SetCooldown(cooldown);
            }
            
            weapon.StartAttacking();
        }
    }

    private void OnDisable()
    {
        foreach (var weapon in _weaponsAndCooldowns)
        {
            weapon.Key.AttackerDetected -= OnAttackerDetected;
        }
    }
    
    public void UpgradeWeapon(Weapon weapon)
    {
        
    }
    
    public void UpgradeWeapon()
    {
        
    }

    public void AddWeapon(Weapon weapon)
    {

    }

    public bool HasWeapon(Weapon weapon)
    {
        return _weaponsAndCooldowns.Contains(weapon);
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
}