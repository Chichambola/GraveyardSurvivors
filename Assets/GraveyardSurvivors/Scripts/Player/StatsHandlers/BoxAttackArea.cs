using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoxAttackArea : AttackArea
{
    private BoxCollider _collider;
    private Collider[] _hitColliders;
    private Vector3 _initialSize;
    
    protected override void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _hitColliders = new Collider[NumberOfColliders];
        _initialSize = _collider.size;
    }

    protected override void OnEnable()
    {
        _collider.size = _initialSize;
    }

    protected override void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public override void SetActive(bool value)
    {
        _collider.enabled = value;
    }

    public override void SetSize(float value, float multiplier = 0f)
    {
        var size = _collider.size;

        if (multiplier != 0)
        {
            size.y = UserUtils.AddPercentToNumber(size.y, multiplier);
            size.z = UserUtils.AddPercentToNumber(size.z, multiplier);   
        }
        
        _collider.size = size;
    }

    public override bool TryGetAttackers(out List<IAttacker> attackers)
    {
        float scaleOffset = 0.5f;
        
        Vector3 detectAreaCenter = _collider.transform.TransformPoint(_collider.center);
        Vector3 detectAreaHalfExtents = Vector3.Scale(_collider.size, _collider.transform.lossyScale) * scaleOffset;

        int hits = Physics.OverlapBoxNonAlloc(detectAreaCenter, detectAreaHalfExtents, _hitColliders, _collider.transform.rotation);
        
        attackers = new List<IAttacker>();
        
        bool isAnyAttackers = TryGetAttackers(ref attackers, _hitColliders, hits);

        return isAnyAttackers;
    }
}
