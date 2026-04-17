using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.Serialization;
using Tween = PrimeTween.Tween;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private MeshRenderer _radiusSphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    [SerializeField] private ParticleEffectSpawner _particleSpawner;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private float _duration;
    private float _radius;
    private Vector3 _targetRadius;
    private Tween _expandingTween;

    private void OnDisable()
    {
        ChangeSpheresVisibility(false);
        _expandingSphere.gameObject.transform.localScale = Vector3.zero;
        _radiusSphere.gameObject.transform.localScale = Vector3.zero;
        _expandingTween.Stop();
    }

    public override void Execute(float radius, float duration)
    {
        _attackArea.SetSize(radius);  
        
        _duration = duration;
        _radius = radius;
        
        _targetRadius = new Vector3(radius, radius, radius);
        _radiusSphere.gameObject.transform.localScale = _targetRadius;
        
        ChangeSpheresVisibility(true);
        
        _expandingTween = Tween.Scale(_expandingSphere.gameObject.transform, _targetRadius, _duration).OnComplete(LookForAttackers);
    }

    private void LookForAttackers()
    {
        ChangeSpheresVisibility(false);
        
        _particleSpawner.Spawn(gameObject.transform.position, _radius);
        
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

    private void ChangeSpheresVisibility(bool value)
    {
        _radiusSphere.enabled = value;
        _expandingSphere.enabled = value;
    }
}
