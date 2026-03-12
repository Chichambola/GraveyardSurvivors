using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponInfo _info;
    [SerializeField] protected AttackStrategy AttackStrategy;
    
    public virtual event Action<IAttacker> AttackerDetected;
    public WeaponInfo Info => _info;
    public virtual bool IsAttacking { get; private set; }

    public virtual void Attack(float duration, float radius) {}
    public virtual void Attack(float duration) {}
}
