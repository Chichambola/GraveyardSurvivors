using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private MeshRenderer _sphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private Coroutine _expandingCoroutine;
    private float _duration;
    private Vector3 _targetRadius;

    private void OnEnable()
    {
        _sphere.enabled = false;
        _expandingSphere.enabled = false;
    }

    public override void Execute(float radius, float duration)
    {
        if (radius > 0)
        {
            _attackArea.SetSize(radius + 1);   
        }
        else
        {
            _attackArea.SetSize(radius);  
        }
        
        _duration = duration;
        
        _targetRadius = new Vector3(radius, radius, radius);
        _sphere.gameObject.transform.localScale = _targetRadius;
        
        _sphere.enabled = true;
        _expandingSphere.enabled = true;
        
        if (_expandingCoroutine != null)
            StopCoroutine(_expandingCoroutine);

        _expandingCoroutine = StartCoroutine(ExpandingCoroutine());
    }

    private IEnumerator ExpandingCoroutine()
    {
        Vector3 scale = Vector3.zero;

        float duration = 0;
        
        while (duration < _duration)
        {
            duration += Time.deltaTime;
            
            scale = Vector3.Lerp(scale, _targetRadius, Time.deltaTime * duration);
            
            _expandingSphere.gameObject.transform.localScale = scale;
            
            yield return null;
        }
        
        LookForAttackers();
    }

    private void LookForAttackers()
    {
        List<IAttacker> attackers = new List<IAttacker>();
        
        _attackArea.TryGetAttackers(out attackers);

        if (attackers != null && attackers.Count > 0)
        {
            foreach (var attacker in attackers)
            {
                AttackerDetected?.Invoke(attacker);
            }
        }
    }
}
