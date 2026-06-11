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

    protected virtual void Awake()
    {
        _validColliders = new Dictionary<Collider, IAttacker>();
    }

    protected abstract void OnEnable();

    protected abstract void OnValidate();

    public virtual void SetActive(bool value) { }

    public virtual void SetSize(float value) {}
    public virtual void SetSize() {}

    public abstract void AddMultiplier(float multiplier);
    
    public abstract bool TryGetAttackers(out List<IAttacker> attackers);
    
    protected bool TryGetAttackers(out List<IAttacker> attackers, Collider[] hitColliders, int hits)
    {
        var tempAttackers = new List<IAttacker>();
        
        for (int i = 0; i < hits; i++)
        {
            if (_validColliders.ContainsKey(hitColliders[i]))
            {
                tempAttackers.Add(_validColliders[hitColliders[i]]);
            }
            else
            {
                if (!hitColliders[i].TryGetComponent(out IAttacker attacker))
                    continue;
                
                _validColliders.Add(hitColliders[i], attacker);
                
                tempAttackers.Add(attacker);
            }
        }

        if (tempAttackers.Count > 0)
        {
            attackers = new List<IAttacker>();
            
            attackers.AddRange(tempAttackers);

            return true;
        }
        else
        {
            attackers = null;

            return false;
        }
    }
}
