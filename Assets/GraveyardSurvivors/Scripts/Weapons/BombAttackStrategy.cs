using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.Serialization;
using Tween = PrimeTween.Tween;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private Ease _ease;
    [SerializeField] private MeshRenderer _radiusSphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private float _duration;
    private Vector3 _targetRadius;
    private Tween _expandingTween;

    private void OnDisable()
    {
        _radiusSphere.enabled = false;
        _expandingSphere.enabled = false;
        _expandingSphere.gameObject.transform.localScale = Vector3.zero;
        _radiusSphere.gameObject.transform.localScale = Vector3.zero;
        _expandingTween.Stop();
    }

    public override void Execute(float radius, float duration)
    {
        _attackArea.SetSize(radius);  
        
        _duration = duration;
        
        _targetRadius = new Vector3(radius, radius, radius);
        _radiusSphere.gameObject.transform.localScale = _targetRadius;
        
        _radiusSphere.enabled = true;
        _expandingSphere.enabled = true;
        
        _expandingTween = Tween.Scale(_expandingSphere.gameObject.transform, _targetRadius, _duration).OnComplete(LookForAttackers);
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
