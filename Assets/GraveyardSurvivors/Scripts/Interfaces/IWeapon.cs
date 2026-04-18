using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public WeaponInfo Info { get; }
    void Attack(float radius);
}
