using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereAttackArea : AttackArea
{
    private SphereCollider _collider;
    private Collider[] _hitColliders;
    private float _initialRadius;
    private readonly float _scaleOffset = 0.5f;
    
    protected override void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _hitColliders = new Collider[NumberOfColliders];
        _initialRadius = _collider.radius;
    }

    protected override void OnEnable()
    {
        _collider.radius = _initialRadius;
    }

    protected override void OnValidate()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        GetComponent<SphereCollider>().transform.localScale = new Vector3(_scaleOffset, _scaleOffset, _scaleOffset);
    }

    public override void SetSize(float value)
    {
        float radius = value;
        
        _collider.radius = radius;
    }

    public override bool TryGetAttackers(out List<IAttacker> attackers)
    {
        int hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _collider.radius * _scaleOffset, _hitColliders, Mask);
        
        attackers = new List<IAttacker>();

        bool isAnyAttackers = TryGetAttackers(ref attackers, _hitColliders, hits);

        return isAnyAttackers;
    }

    public override void AddMultiplier(float multiplier)
    {
        if (multiplier != 0)
        {
            _collider.radius = _collider.radius.AddPercentToNumber(multiplier);   
        }
    }
}
