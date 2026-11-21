using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MeleeAttackStrategy : AttackStrategy
{
    [SerializeField] private BoxAttackArea _area;
    
    public override event Action<IAttacker> AttackerDetected;

    public override void Execute()
    {
        if (_area.TryGetAttackers(out List<IAttacker> detectedAttackers))
        {
            foreach (var attacker in detectedAttackers)
            {
                AttackerDetected?.Invoke(attacker);   
            }
        }
    }

    public void Upgrade(float gain)
    {
        _area.Upgrade(gain);
    }

    public void SetActive(bool value)
    {
        _area.SetActive(value);
    }
}