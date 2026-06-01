using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public event Action<IAttacker, Weapon> AttackerDetected;
    public float Damage { get; }
    void Attack();
    void Upgrade();
}
