using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon, IItem
{
    [SerializeField] private WeaponInfo _info;
    [SerializeField] private float _bonusDamagePerUpgrade = 1;
    
    protected float BonusDamage;
    protected float Cooldown;
    
    public virtual event Action<IAttacker, Weapon> AttackerDetected;
    
    public Sprite Sprite => _info.Sprite;
    public float Damage => _info.Damage + BonusDamage;
    public string Name => _info.Name;
    public string BaseDescription => _info.BaseDescription;
    public abstract string UpgradeDescription { get; protected set; }
    public string CurrentDescription { get; private set; }
    public bool IsAttacking { get; protected set; }

    public virtual void Attack()
    {
        
    }

    private void OnDisable()
    {
        BonusDamage = 0;
    }

    public virtual void Upgrade()
    {
        BonusDamage += _bonusDamagePerUpgrade;
    }

    public virtual void Reset()
    {
        
    }

    public void SetDescription(string description)
    {
        CurrentDescription = description;
    }

    public void SetCooldown(float cooldown)
    {
        Cooldown = cooldown;
    }

    public virtual void StartAttacking()
    {
        
    }
    
    public virtual void StopAttacking()
    {
        
    }
}
