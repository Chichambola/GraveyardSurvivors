using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Weapon : Item, IWeapon
{
    [Header("Weapon specific fields")]
    [SerializeField] protected float BonusDamagePerUpgrade = 1;
    [SerializeField] protected float Cooldown = 1f;
    [SerializeField] private float _damage;
    
    protected float BonusDamage;
    private string _description;
    
    public virtual event Action<IAttacker, IWeapon> AttackerDetected;
    
    public float Damage => _damage + BonusDamage;
    public float CurrentCooldown => Cooldown;
    public abstract string BaseDescription { get; }
    public abstract string UpgradeDescription { get; }
    public override string CurrentDescription => _description;
    public bool IsAttacking { get; protected set; }

    private void OnDisable()
    {
        BonusDamage = 0;
    }

    public virtual void Upgrade()
    {
        BonusDamage += BonusDamagePerUpgrade;
    }

    public virtual void Reset() { }

    public void SetDescription(string description)
    {
        _description = description;
    }

    public virtual void SetCooldown(float cooldown)
    {
        Cooldown = cooldown;
    }

    public virtual void StartAttacking() { }
    
    public virtual void StopAttacking() {}
    
    public virtual void Attack() { }
}
