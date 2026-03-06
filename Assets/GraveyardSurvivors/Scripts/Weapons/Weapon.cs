using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponInfo _info;

    public virtual event Action<Weapon> FinishedAttacking;
    public virtual event Action<IAttacker> AttackerDetected;
    
    public WeaponInfo Info => _info;

    public virtual void Attack(float duration, float radius) {}
    public virtual void Attack(float duration) {}
    public virtual void StopAttacking() { }
}
