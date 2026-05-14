using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public Vector3 CurrentPosition { get; }
    public bool IsAlive { get; }
    void TakeDamage(float damage);
    void ApplyEffect(IEffect<IAttacker> effectFactory);
    void ChangeSpeed(float speedPercent, bool isSlowing);
}
