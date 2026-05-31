using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected AttackStrategy AttackStrategy;
    [SerializeField] private WeaponInfo _info;
    
    protected float BonusDamage;
    
    public virtual event Action<IAttacker> AttackerDetected;
    public float Damage => _info.Damage + BonusDamage;

    public abstract void Attack(float radiusMultiplier = 0f);
    public virtual void StopAttacking() {}

    private void OnDisable()
    {
        BonusDamage = 0;
    }

    public virtual void Upgrade()
    {
        //noop
    }
}
