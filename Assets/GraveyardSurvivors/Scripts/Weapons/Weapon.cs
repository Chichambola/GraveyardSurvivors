using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon, IItem
{
    [SerializeField] protected AttackStrategy AttackStrategy;
    [SerializeField] private WeaponInfo _info;
    [SerializeField] private float _bonusDamagePerUpgrade = 1;
    
    protected float BonusDamage;
    
    public virtual event Action<IAttacker, Weapon> AttackerDetected;
    
    public Sprite Sprite => _info.Sprite;
    public float Damage => _info.Damage + BonusDamage;
    public string Name => _info.Name;
    public string Description => _info.Description;

    public abstract void Attack();

    private void OnDisable()
    {
        BonusDamage = 0;
    }

    public virtual void Upgrade()
    {
        AttackStrategy.Upgrade();
        BonusDamage += _bonusDamagePerUpgrade;
    }

    public virtual void Reset()
    {
        
    }
}
