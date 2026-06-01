using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected AttackStrategy AttackStrategy;
    [SerializeField] private WeaponInfo _info;
    [SerializeField] private float _bonusDamagePerUpgrade = 1;
    
    protected float BonusDamage;
    
    public virtual event Action<IAttacker, Weapon> AttackerDetected;
    public float Damage => _info.Damage + BonusDamage;

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
        AttackStrategy.Reset();
    }
}
