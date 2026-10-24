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
    private List<IAttacker> _sortedAttackers;
    private IAttacker _closestAttacker;

    public int ProjectilePerUpgrade => _projectilePerUpgrade;
    public int InitialProjectileAmount => _initialProjectileAmount;

    private void Awake()
    {
        _currentProjectileAmount = _initialProjectileAmount;

        _sortedAttackers = new List<IAttacker>();
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
                    
                    if (_count > _currentProjectileAmount) 
                        return;
                }
            }
            
            if (_count < _currentProjectileAmount) 
                continue;
            
            _count = 0;

            return;
        }
    }

    private bool TryFindClosestAttackers(AttackArea attackArea, out List<IAttacker> sortedAttackers)
    {
        _sortedAttackers.Clear();
        
        if (attackArea.TryGetAttackers(out List<IAttacker> currentAttackers))
        {
            for (int i = 0; i < _currentProjectileAmount; i++)
            {
                float minDistance = float.MaxValue;

                _closestAttacker = null;

                foreach (var attacker in currentAttackers)
                {
                    var targetPosition = attacker.CurrentPosition;
                    
                    float distance = Vector3.Distance(gameObject.transform.position, targetPosition);

                    if (distance < minDistance)
                    {
                        minDistance = distance;

                        _closestAttacker = attacker;
                    }
                }
                
                if (_closestAttacker != null && !_sortedAttackers.Contains(_closestAttacker))
                {
                    _sortedAttackers.Add(_closestAttacker);

                    currentAttackers.Remove(_closestAttacker);
                }
            }
        }

        if (_sortedAttackers.Count > 0)
        {
            sortedAttackers = _sortedAttackers;
            
            return true;
        }
        else
        {
            sortedAttackers = null;

            return false;
        }
    }
}