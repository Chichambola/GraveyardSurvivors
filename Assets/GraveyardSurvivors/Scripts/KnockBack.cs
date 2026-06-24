using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

public class KnockBack : MonoBehaviour, IEffect<IAttacker>
{
    [SerializeField] private float _initialForce = 2f;
    [SerializeField] private float _duration;
    [SerializeField] private float _knockBackPercentGain = 2f;
    
    private float _force;

    public event Action<IEffect<IAttacker>> EffectCompleted;
    
    public float KnockBackPercentGain => _knockBackPercentGain;

    private void OnEnable()
    {
        _force = _initialForce;
    }
    
    public void Upgrade()
    {
        _force = _force.AddPercentToNumber(_knockBackPercentGain);
    }
    
    public void Apply(IAttacker attacker)
    {
        var pushableAttacker = attacker as MonoBehaviour;

        if (pushableAttacker == null)
            throw new Exception("Attacker is not MonoBehaviour");
        
        var pushDirection = (pushableAttacker.transform.position - gameObject.transform.position).normalized;

        Vector3 adjustedTarget = pushableAttacker.transform.position + new Vector3(pushDirection.x, 0, pushDirection.z) * _force; 
        
        Tween.Position(pushableAttacker.transform, adjustedTarget, _duration).OnComplete(() => EffectCompleted?.Invoke(this));
    }

    public void Cancel() { }
}
