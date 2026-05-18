using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public float Damage { get; }
    void Attack(float radius);
}
