using System;
using System.Collections.Generic;
using UnityEngine;
using Tween = PrimeTween.Tween;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private ParticleEffectSpawner _particleSpawner;
    [SerializeField] private MeshRenderer _radiusSphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    [SerializeField] private float _duration;
    [SerializeField] private float _radius;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private Vector3 _targetRadius;
    private Tween _expandingTween;

    private void OnDisable()
    {
        ChangeSpheresVisibility(false);
        _expandingSphere.gameObject.transform.localScale = Vector3.zero;
        _radiusSphere.gameObject.transform.localScale = Vector3.zero;
        _expandingTween.Stop();
    }

    public override void Execute(float radiusMultiplier)
    {
        _attackArea.SetSize(_radius);  
        _attackArea.AddMultiplier(radiusMultiplier);
        
        _targetRadius = new Vector3(_radius, _radius, _radius);
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
        
        _expandingTween.Stop();
    }

    private void ChangeSpheresVisibility(bool value)
    {
        _radiusSphere.enabled = value;
        _expandingSphere.enabled = value;
    }
}
