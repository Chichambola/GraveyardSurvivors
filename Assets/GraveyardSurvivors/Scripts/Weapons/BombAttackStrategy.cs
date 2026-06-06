using System;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEditor.Experimental;
using UnityEngine.Serialization;

public class BombAttackStrategy : AttackStrategy
{
    [SerializeField] private AttackArea _attackArea;
    [SerializeField] private ParticleEffectSpawner _particleSpawner;
    [SerializeField] private MeshRenderer _radiusSphere;
    [SerializeField] private MeshRenderer _expandingSphere;
    [SerializeField] private float _initialDuration = 1.5f;
    [SerializeField] private float _initialRadius = 10f;
    [Header("Upgrade stats")]
    [SerializeField] private float _upgradeDurationPercent = 20;
    [SerializeField] private float _minDurationThreshold = 1;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private Vector3 _targetRadius;
    private Tween _expandingTween;
    private bool _isExpanding;
    private float _currentDuration;

    private void Awake()
    {
        _currentDuration = _initialDuration;
    }

    private void OnValidate()
    {
        if (_minDurationThreshold > _initialDuration)
        {
            _minDurationThreshold = _initialDuration;
        }

        if (_minDurationThreshold < 0)
        {
            _minDurationThreshold = 0;
        }
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
        _attackArea.SetSize(_initialRadius);  
        
        _targetRadius = new Vector3(_initialRadius, _initialRadius, _initialRadius);
        _radiusSphere.gameObject.transform.localScale = _targetRadius;
        
        ChangeSpheresVisibility(true);
        
        _expandingTween = Tween.Scale(_expandingSphere.gameObject.transform, _targetRadius, _currentDuration).OnComplete(LookForAttackers);
    }

    public override void Upgrade()
    {
        _currentDuration = _currentDuration.GetClampedValueInverse(_upgradeDurationPercent, _minDurationThreshold);
    }

    public override void Reset()
    {
        _currentDuration = _initialDuration;
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
        _particleSpawner.Spawn(gameObject.transform.position, _initialRadius);
        
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
