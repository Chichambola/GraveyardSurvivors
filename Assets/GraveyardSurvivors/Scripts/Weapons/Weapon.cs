using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected AttackStrategy AttackStrategy;
    [SerializeField] private WeaponInfo _info;
    
    public virtual event Action<IAttacker> AttackerDetected;
    public WeaponInfo Info => _info;
    public virtual bool IsAttacking { get; protected set; }

    public virtual void Attack(float duration, float radius) {}
    public virtual void Attack(float duration) {}
}
