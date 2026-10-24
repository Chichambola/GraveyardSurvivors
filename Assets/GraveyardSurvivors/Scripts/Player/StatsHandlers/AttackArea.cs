using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AttackArea : MonoBehaviour
{
    [SerializeField] protected LayerMask Mask;
    [SerializeField] protected int NumberOfColliders = 50;

    private Dictionary<Collider, IAttacker> _validColliders;
    private List<IAttacker> _tempAttackers;

    protected virtual void Awake()
    {
        _validColliders = new Dictionary<Collider, IAttacker>();
        _tempAttackers = new List<IAttacker>(NumberOfColliders);
    }
    
    public abstract void Upgrade(float multiplier = 0f);
    
    public abstract bool TryGetAttackers(out List<IAttacker> attackers);
    
    protected bool TryGetAttackers(out List<IAttacker> attackers, Collider[] hitColliders, int hits)
    {
        _tempAttackers.Clear();
        
        for (int i = 0; i < hits; i++)
        {
            if (_validColliders.ContainsKey(hitColliders[i]))
            {
                _tempAttackers.Add(_validColliders[hitColliders[i]]);
            }
            else
            {
                if (!hitColliders[i].TryGetComponent(out IAttacker attacker))
                    continue;
                
                _validColliders.Add(hitColliders[i], attacker);
                
                _tempAttackers.Add(attacker);
            }
        }

        if (_tempAttackers.Count > 0)
        {
            attackers = new List<IAttacker>();
            
            attackers.AddRange(_tempAttackers);

            return true;
        }
        else
        {
            attackers = null;

            return false;
        }
    }
}
