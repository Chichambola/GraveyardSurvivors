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

    public abstract void Attack(float radiusMultiplier);
}
