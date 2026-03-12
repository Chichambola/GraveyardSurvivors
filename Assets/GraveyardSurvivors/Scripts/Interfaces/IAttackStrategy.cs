using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackStrategy
{
    void Execute(float radius);
    public event Action<List<IAttacker>> AttackerDetected; 
}
