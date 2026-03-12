using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackArea : MonoBehaviour
{
    [SerializeField] private int _numberOfCollider = 10;
    
    public event Action<IAttacker> EnemyDetected;
    
    private BoxCollider _collider;
    private Collider[] _hitColliders;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _hitColliders = new Collider[_numberOfCollider];
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IAttacker attacker))
        {
            EnemyDetected?.Invoke(attacker);
        }
    }

    public void SetVisibility(bool value)
    {
        _collider.enabled = value;
    }

    public void SetSize(float value)
    {
        var size = _collider.size;
        
        size.x = UserUtils.AddPercentToNumber(size.x, value);
        size.y = UserUtils.AddPercentToNumber(size.x, value);
        
        Vector3 newSize = new Vector3(size.x, _collider.size.y, size.z);
        
        _collider.size = newSize;
    }
    
    public bool TryGetAttackers(out List<IAttacker> attackers)
    {
        float scaleOffset = 0.5f;
        
        Vector3 detectAreaCenter = _collider.transform.TransformPoint(_collider.center);
        Vector3 detectAreaHalfExtents = Vector3.Scale(_collider.size, _collider.transform.lossyScale) * scaleOffset;

        int hits = Physics.OverlapBoxNonAlloc(detectAreaCenter, detectAreaHalfExtents, _hitColliders, _collider.transform.rotation);
        
        attackers = new List<IAttacker>();
        
        for (int i = 0; i < hits; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IAttacker attacker))
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
