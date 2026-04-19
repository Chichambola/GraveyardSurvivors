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
    
    public event Action<IAttacker> EnemyDetected;
    
    protected abstract void Awake();

    protected abstract void OnEnable();

    protected abstract void OnValidate();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IAttacker attacker))
        {
            EnemyDetected?.Invoke(attacker);
        }
    }

    public virtual void SetActive(bool value) { }

    public virtual void SetSize(float value) {}
    public virtual void SetSize() {}

    public abstract bool TryGetAttackers(out List<IAttacker> attackers);

    public abstract void AddMultiplier(float multiplier);
    
    protected bool TryGetAttackers(ref List<IAttacker> attackers, Collider[] hitColliders, int hits)
    {
        for (int i = 0; i < hits; i++)
        {
            if (hitColliders[i].TryGetComponent(out IAttacker attacker))
            {
                attackers.Add(attacker);
            }
        }

        if (attackers.Count <= 0)
        {
            attackers = null;

            return false;
        }
        else
        {
            return true;
        }
    }
}
