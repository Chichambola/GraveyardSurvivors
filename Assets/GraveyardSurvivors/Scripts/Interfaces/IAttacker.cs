using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public Vector3 CurrentPosition { get; }
    public bool IsAlive { get; }
    public float Speed { get; }
    public float CritChance { get; }
    public float CritMultiplier { get; }
    public float Luck { get; }
    void TakeDamage(float damage);
    void ApplyEffect(IEffect<IAttacker> effectFactory);
    void ChangeSpeed(float speedPercent, bool isSlowing);
}
