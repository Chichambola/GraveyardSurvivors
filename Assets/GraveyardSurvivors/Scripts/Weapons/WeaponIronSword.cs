using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponIronSword : WeaponWithAbility
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    [SerializeField] private AttackArea _area;
    [SerializeField] private ParticleSystem _attackParticles;

    private Coroutine _coroutine;
    private float _waitTime = 0.1f;
    
    public override string UpgradeDescription { get; protected set; }

    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(VisibilityRoutine());
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
    }
    
    public override void Attack()
    {
        _attackStrategy.Execute();
    }

    public override void Upgrade()
    {
        _attackStrategy.Upgrade();
        
        base.Upgrade();
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

    private void OnAttackerDetected(IAttacker attacker)
    {
        if (attacker is Enemy _)
        {
            ProcessAttacker(attacker);
        }
    }

    private IEnumerator VisibilityRoutine()
    {
        var wait = new WaitForSecondsRealtime(_waitTime);

        while (enabled)
        {
            yield return wait;

            _area.SetActive(_attackParticles.particleCount > 0);
        }
    }
}