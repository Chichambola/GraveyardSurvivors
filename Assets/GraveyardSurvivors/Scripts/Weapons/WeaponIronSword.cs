using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponIronSword : Weapon
{
    [SerializeField] private AttackArea _area;
    [SerializeField] private ParticleSystem _slash;
    [SerializeField] private Effect[] _bleedingEffect;
    [SerializeField] private float _effectChance;

    public override event Action<IAttacker> AttackerDetected; 
    
    private float _waitTime = 0.1f;
    private Coroutine _slashCoroutine;
    
    private void OnEnable()
    {
        if(_slashCoroutine != null)
            StopCoroutine(_slashCoroutine);

        _slashCoroutine = StartCoroutine(VisibilityCoroutine());
    }

    public override void Attack(float duration, float radius)
    {
        SetParticleSystemDuration(duration);
        
        _area.SetSize(radius);
        
        _slash.Play();
        
        if (_area.TryGetAttacker(out IAttacker attacker))
        {
            AttackerDetected?.Invoke(attacker);
            
            if (CanEffectProc())
            {
                foreach (var effect in _bleedingEffect)
                {
                    effect.Execute(attacker);
                }
            }
        }
    }

    private void SetParticleSystemDuration(float duration)
    {
        _slash.Stop();
        
        if (_slash.isPlaying)
            return;   
        
        var slashMain = _slash.main;

        slashMain.duration = duration;
    }

    private IEnumerator VisibilityCoroutine()
    {
        var wait = new WaitForSeconds(_waitTime);
        
        while (enabled)
        {
            yield return wait;
            
            _area.SetVisibility(_slash.particleCount > 0);
        }
    }

    private bool CanEffectProc()
    {
        float randomNumber = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);

        return _effectChance >= randomNumber;
    }
}
