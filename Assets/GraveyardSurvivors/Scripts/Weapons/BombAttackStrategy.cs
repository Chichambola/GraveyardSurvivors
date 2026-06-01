using System;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEditor.Experimental;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private ParticleEffectSpawner _particleSpawner;
    [SerializeField] private MeshRenderer _radiusSphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    [SerializeField] private float _duration = 1.5f;
    [SerializeField] private float _radius = 10f;
    [Header("Upgrade stats")]
    [SerializeField] private float _upgradeDurationPercent = 20;
    [SerializeField] private float _upgradeRadiusPercent = 15;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private Vector3 _targetRadius;
    private Tween _expandingTween;
    private bool _isExpanding;
    private float _initialRadius;
    private float _initialDuration;

    private void Awake()
    {
        _initialDuration = _duration;
        _initialRadius = _radius;
        
        Debug.Log(_duration);
    }

    private void OnDisable()
    {
        if (_isExpanding)
        {
            Stop();
        }
    }

    public override void Execute()
    {
        if (_isExpanding) return;
        
        _isExpanding = true;
        _attackArea.SetSize(_radius);  
        
        _targetRadius = new Vector3(_radius, _radius, _radius);
        _radiusSphere.gameObject.transform.localScale = _targetRadius;
        
        ChangeSpheresVisibility(true);
        
        _expandingTween = Tween.Scale(_expandingSphere.gameObject.transform, _targetRadius, _duration).OnComplete(LookForAttackers);
    }

    public override void Upgrade()
    {
        _duration = _duration.GetClampedValueInverse(_upgradeDurationPercent);
    }

    public override void Reset()
    {
        Debug.Log($"Before duration: {_duration}");
        
        _duration = _initialDuration;
        _radius = _initialRadius;
        
        Debug.Log($"After duration: {_duration}");
    }

    private void Stop()
    {
        ChangeSpheresVisibility(false);
        _isExpanding = false;
        _expandingSphere.gameObject.transform.localScale = Vector3.zero;
        _radiusSphere.gameObject.transform.localScale = Vector3.zero;
        _expandingTween.Stop();
    }
    
    private void LookForAttackers()
    {
        _particleSpawner.Spawn(gameObject.transform.position, _radius);
        
        if (_attackArea.TryGetAttackers(out var attackers))
        {
            foreach (var attacker in attackers)
            {
                AttackerDetected?.Invoke(attacker);
            }
        }
        
        Stop();
    }

    private void ChangeSpheresVisibility(bool value)
    {
        _radiusSphere.enabled = value;
        _expandingSphere.enabled = value;
    }
}
