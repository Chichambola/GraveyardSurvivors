using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private List<Weapon> _initialWeapons;
    [SerializeField] private float _cooldown = 1.5f;
    
    private Coroutine _coroutine;
    private IAttacker _attacker;
    private List<Weapon> _currentWeapons;
    
    public bool IsAttacking {get; private set;}

    public void Init(IAttacker attacker)
    {
        _attacker = attacker;
    }

    private void Awake()
    {
        _currentWeapons = new List<Weapon>();
        
        _currentWeapons.AddRange(_initialWeapons);
    }

    private void OnEnable()
    {
        foreach (var weapon in _initialWeapons)
        {
            var currentWeapon = Instantiate(weapon, transform.position, Quaternion.identity, transform);
            
            _currentWeapons.Add(currentWeapon);
            
            currentWeapon.AttackerDetected += OnAttackerDetected;
        }
    }

    private void OnDisable()
    {
        foreach (var weapon in _currentWeapons)
        {
            weapon.AttackerDetected -= OnAttackerDetected;
        }
        
        _currentWeapons.Clear();
    }

    public void StartAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(AttackingCoroutine());
    }
    
    public void StopAttacking()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void Reset()
    {
        foreach (var weapon in _currentWeapons)
        {
            weapon.Reset();
        }
    }
    
    public void UpgradeWeapon(Weapon weapon)
    {
        _currentWeapons.FirstOrDefault(w => w == weapon)?.Upgrade();
    }
    
    public void UpgradeWeapon()
    {
        _currentWeapons.First().Upgrade();
    }

    public void AddWeapon(IWeapon item)
    {
        if (item is not Weapon weapon) 
            return;
        
        var currentWeapon = Instantiate(weapon, transform.position, Quaternion.identity, transform);
        
        _currentWeapons.Add(currentWeapon);
    }

    public bool HasWeapon(Weapon weapon) => _currentWeapons.Contains(weapon);
    
    private IEnumerator AttackingCoroutine()
    {
        IsAttacking = true;
        
        float currentCooldown = _cooldown;
        
        while (enabled)
        {
            currentCooldown = CompareCooldown(currentCooldown);
            
            var wait = new WaitForSeconds(currentCooldown);
            
            yield return wait;

            foreach (var weapon in _currentWeapons)
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
}