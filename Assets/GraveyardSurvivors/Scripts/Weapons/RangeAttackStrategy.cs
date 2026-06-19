using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Serialization;

public class RangeAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea[] _attackAreas;
    [SerializeField] private int _initialProjectileAmount = 2;
    [SerializeField] private int _projectilePerUpgrade;

    public override event Action<IAttacker> AttackerDetected;

    private int _count;
    private int _currentProjectileAmount;

    public int ProjectilePerUpgrade => _projectilePerUpgrade;

    private void OnEnable()
    {
        _currentProjectileAmount = _initialProjectileAmount;
    }

    public override void Upgrade()
    {
        _currentProjectileAmount += _projectilePerUpgrade;
    }

    public override void Execute()
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
            
            if (_count < _initialProjectileAmount) continue;
            
            _count = 0;

            return;
        }
    }

    private bool TryFindClosestAttackers(AttackArea attackArea, out List<IAttacker> sortedAttackers)
    {
        sortedAttackers = new List<IAttacker>();
        
        if (attackArea.TryGetAttackers(out List<IAttacker> currentAttackers))
        {
            for (int i = 0; i < _initialProjectileAmount; i++)
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