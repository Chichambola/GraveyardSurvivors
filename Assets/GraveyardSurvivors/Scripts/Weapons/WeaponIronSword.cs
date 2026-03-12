using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponIronSword : Weapon
{
    [SerializeField] private AttackArea _area;
    [SerializeField] private ParticleSystem _attackParticles;
    [SerializeField] private Effect[] _bleedingEffect;
    [SerializeField] private float _effectChance;

    private Coroutine _coroutine;
    private float _waitTime = 0.1f;
    
    public override event Action<IAttacker> AttackerDetected;
    
    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(VisibilityRoutine());
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Attack(float duration, float radius)
    {
        SetParticleSystemDuration(duration);
        
        AttackStrategy.Execute(radius);
    }

    private bool CanEffectProc()
    {
        float randomNumber = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);

        return _effectChance >= randomNumber;
    }

    private void SetParticleSystemDuration(float duration)
    {
        _attackParticles.Stop();
        
        if (_attackParticles.isPlaying)
            return;   
        
        var slashMain = _attackParticles.main;

        slashMain.duration = duration;
        
        _attackParticles.Play();
    }
    
    private void OnAttackerDetected(List<IAttacker> attackers)
    {
        foreach (var attacker in attackers)
        {
            if (attacker is Enemy _)
            {
                ProcessAttacker(attacker);
            }
        }
    }

    private void ProcessAttacker(IAttacker attacker)
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

    private IEnumerator VisibilityRoutine()
    {
        var wait = new WaitForSecondsRealtime(_waitTime);

        while (enabled)
        {
            yield return wait;
            
            _area.SetVisibility(_attackParticles.particleCount > 0);
        }
    }
}
