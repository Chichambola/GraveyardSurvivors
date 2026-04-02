using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    public Rigidbody Rigidbody { get; }
    void TakeDamage(float damage);
    void ApplyEffect(IEffect<IAttacker> effectFactory);
    void ChangeSpeed(float speedPercent, bool isSlowing);
}
