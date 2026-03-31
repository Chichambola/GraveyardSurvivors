using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackStrategy : MonoBehaviour, IAttackStrategy
{
    public abstract event Action<IAttacker> AttackerDetected;
    public abstract void Execute(float radius = 0f);
}
