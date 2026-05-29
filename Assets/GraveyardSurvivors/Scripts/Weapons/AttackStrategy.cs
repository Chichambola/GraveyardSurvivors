using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    public abstract event Action<IAttacker> AttackerDetected;
    public virtual void Execute(float radiusMultiplier) {}
    public virtual void Stop() { }
    public virtual void Upgrade() { }
}
