using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using UnityEngine;

[Serializable]
public class DamageOvertime : IEffect<Enemy>
{
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _tickInterval = 1f;
    [SerializeField] private float _damagePerTick = 1f;

    private Enemy _currentTarget;
    private Coroutine _coroutine;
    
    public void Apply(Enemy attacker)
    {
        _currentTarget = attacker;

        /*if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DealingDamage());*/
    }

    public void Cancel()
    {
        _currentTarget = null;
        //StopCoroutine(_coroutine);
    }

    private IEnumerator DealingDamage()
    {
        var wait = new WaitForSeconds(_tickInterval);

        float currentTime = 0f;
        
        while (currentTime < _duration)
        {
            Debug.Log("Dealing damage");
            
            _currentTarget.TakeDamage(_damagePerTick);

            yield return wait;
        }
        
        Cancel();
    }
}
