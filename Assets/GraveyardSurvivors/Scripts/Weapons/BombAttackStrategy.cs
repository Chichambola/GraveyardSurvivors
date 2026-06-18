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
    [SerializeField] private float _minDurationThreshold = 1f;
    [SerializeField] private float _upgradeRadiusPercent = 20f;
    [SerializeField] private float _maxRadiusThreshold = 15f;
    
    public override event Action<IAttacker> AttackerDetected;
    public override event Action AttackExecuted;

    private Vector3 _targetRadius;
    private Tween _expandingTween;
    private bool _isExpanding;
    private float _currentDuration;
    private float _currentRadius;

    private void Awake()
    {
        _currentDuration = _initialDuration;
        _currentRadius = _initialRadius;
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

        if (_initialRadius < 0)
        {
            _initialRadius = 0;
        }

        if (_initialRadius > _maxRadiusThreshold)
        {
            _initialRadius = _maxRadiusThreshold;
        }

        if (_maxRadiusThreshold <= _initialRadius)
        {
            _maxRadiusThreshold = _initialRadius + 1;
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
        _attackArea.SetSize(_currentRadius);  
        
        _targetRadius = new Vector3(_currentRadius, _currentRadius, _currentRadius);
        _radiusSphere.gameObject.transform.localScale = _targetRadius;
        
        ChangeSpheresVisibility(true);
        
        _expandingTween = Tween.Scale(_expandingSphere.gameObject.transform, _targetRadius, _currentDuration).OnComplete(LookForAttackers);
    }

    public override void Upgrade()
    {
        float tempDurationPercent = _currentDuration.GetClampedValueInverse(_upgradeDurationPercent, _minDurationThreshold);
        float tempRadiusPercent = _currentRadius.GetClampedValue(_upgradeRadiusPercent, _maxRadiusThreshold);

        _currentDuration = _currentDuration.AddPercentToNumber(tempDurationPercent);
        _currentRadius = _currentRadius.AddPercentToNumber(tempRadiusPercent);
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
        
        AttackExecuted?.Invoke();
    }

    private void ChangeSpheresVisibility(bool value)
    {
        _radiusSphere.enabled = value;
        _expandingSphere.enabled = value;
    }
}
