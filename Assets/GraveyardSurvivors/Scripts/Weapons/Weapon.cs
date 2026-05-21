using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected AttackStrategy AttackStrategy;
    [SerializeField] private WeaponInfo _info;
    
    private float _damageIncrease;

    public virtual event Action<IAttacker> AttackerDetected;
    public float Damage => _info.Damage + _damageIncrease;

    public abstract void Attack(float radiusMultiplier = 0f);
    public virtual void StopAttacking() {}

    private void OnDisable()
    {
        _damageIncrease = 0;
    }

    public void IncreaseDamage(float damage)
    {
        _damageIncrease += damage;
    }
}
