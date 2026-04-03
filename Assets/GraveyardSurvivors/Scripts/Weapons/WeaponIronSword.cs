using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponIronSword : WeaponWithAbility
{
    [SerializeField] private AttackArea _area;
    [SerializeField] private ParticleSystem _attackParticles;
    [SerializeField] private Effect[] _bleedingEffect;
    [SerializeField] private float _effectChance;

    private Coroutine _coroutine;
    private float _lastDuration;
    private float _waitTime = 0.1f;

    public override event Action<IAttacker> AttackerDetected;

    private void OnEnable()
    {
        AttackStrategy.AttackerDetected += OnAttackerDetected;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(VisibilityRoutine());
    }

    private void OnDisable()
    {
        AttackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Attack(float duration, float radius)
    {
        if (!Mathf.Approximately(duration, _lastDuration))
        {
            SetParticleSystemDuration(duration);
        }

        AttackStrategy.Execute(radius);
    }

    private void SetParticleSystemDuration(float duration)
    {
        _attackParticles.Stop();

        if (_attackParticles.isPlaying)
            return;

        var slashMain = _attackParticles.main;

        slashMain.duration = duration;

        _lastDuration = duration;

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

            _area.SetVisibility(_attackParticles.particleCount > 0);
        }
    }
}