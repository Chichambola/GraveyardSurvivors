using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AttackArea : MonoBehaviour
{
    public event Action<IAttacker> EnemyDetected;
    
    private BoxCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
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

    public bool TryGetAttacker(out IAttacker attacker)
    {
        float scaleOffset = 0.5f;
        
        Vector3 detectAreaCenter = _collider.transform.TransformPoint(_collider.center);
        Vector3 detectAreaHalfExtents = Vector3.Scale(_collider.size, _collider.transform.lossyScale) * scaleOffset;

        Collider[] hitColliders =
            Physics.OverlapBox(detectAreaCenter, detectAreaHalfExtents, _collider.transform.rotation);

        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent(out IAttacker tempAttacker))
            {
                attacker = tempAttacker;

                return true;
            }
        }

        attacker = null;

        return false;
    }
}
