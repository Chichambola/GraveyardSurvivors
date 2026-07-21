using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public event Action<IAttacker, IWeapon> AttackerDetected;
    public float Damage { get; }
    public bool IsAttacking { get; }
    void Attack();
    void StopAttacking();
    void Upgrade();
}
