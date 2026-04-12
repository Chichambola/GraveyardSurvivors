using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private int _numberOfColliders = 10;
    
    public event Action<IAttacker> EnemyDetected;
    
    private BoxCollider _collider;
    private Collider[] _hitColliders;
    private Vector3 _initialSize;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _hitColliders = new Collider[_numberOfColliders];
        _initialSize = _collider.size;
    }

    private void OnEnable()
    {
        _collider.size = _initialSize;
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

    public void SetActive(bool value)
    {
        _collider.enabled = value;
    }

    public void SetSize(float value)
    {
        var size = _collider.size;
        
        size.x = UserUtils.AddPercentToNumber(size.x, value);
        size.z = UserUtils.AddPercentToNumber(size.z, value);
        
        _collider.size = size;
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
