using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class RangeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea[] _attackAreas;
    [SerializeField] private int _numberOfProjectiles = 2;

    public override event Action<IAttacker> AttackerDetected;

    private int _count;

    public override void Upgrade()
    {
        _numberOfProjectiles++;
    }

    public override void Execute(float radiusMultiplier)
    {
        foreach (var attackArea in _attackAreas)
        {
            if (TryFindClosestAttackers(attackArea, out List<IAttacker> sortedAttackers))
            {
                _count += sortedAttackers.Count;
                
                foreach (var attacker in sortedAttackers)
                {
                    AttackerDetected?.Invoke(attacker);
                }
            }
            
            if (_count < _numberOfProjectiles) continue;
            
            _count = 0;

            return;
        }
    }

    private bool TryFindClosestAttackers(AttackArea attackArea, out List<IAttacker> sortedAttackers)
    {
        sortedAttackers = new List<IAttacker>();
        
        if (attackArea.TryGetAttackers(out List<IAttacker> currentAttackers))
        {
            for (int i = 0; i < _numberOfProjectiles; i++)
            {
                float minDistance = float.MaxValue;

                IAttacker closestAttacker = null;

                foreach (var attacker in currentAttackers)
                {
                    var targetPosition = attacker.CurrentPosition;
                    
                    float distance = Vector3.Distance(gameObject.transform.position, targetPosition);

                    if (distance < minDistance)
                    {
                        minDistance = distance;

                        closestAttacker = attacker;
                    }
                }
                
                if (closestAttacker != null)
                {
                    sortedAttackers.Add(closestAttacker);

                    currentAttackers.Remove(closestAttacker);
                }
            }
        }

        if (sortedAttackers.Count > 0)
        {
            return true;
        }
        else
        {
            sortedAttackers = null;

            return false;
        }
    }
}