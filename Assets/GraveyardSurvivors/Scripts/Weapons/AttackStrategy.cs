using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour
{
    public abstract event Action<IAttacker> AttackerDetected;
    public abstract void Execute();
    public abstract void Upgrade();
    public virtual void Reset() { }
}
