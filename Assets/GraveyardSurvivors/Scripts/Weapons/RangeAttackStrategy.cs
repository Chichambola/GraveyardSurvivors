using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class RangeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea[] _attackAreas;
    [SerializeField] private int _numberOfProjectiles = 2;

    public override event Action<IAttacker> AttackerDetected;

    private IAttacker _closestInteractable;

    public override void Execute(float radius = 0)
    {
        foreach (var attackArea in _attackAreas)
        {
            if (TryFindClosestAttackers(attackArea, out List<IAttacker> sortedAttackers))
            {
                foreach (var attacker in sortedAttackers)
                {
                    AttackerDetected?.Invoke(attacker);
                }
                
                return;
            }
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
                    float distance = Vector3.Distance(gameObject.transform.position, attacker.Rigidbody.position);

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