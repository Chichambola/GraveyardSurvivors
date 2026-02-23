using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] protected WeaponInfo _info; 
    
    public WeaponInfo Info => _info;

    public abstract void Attack(float duration);
}
