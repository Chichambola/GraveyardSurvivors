using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    public event Action<IAttacker> AttackerDetected;
    void Execute(float radius);
}
