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

    private Coroutine _coroutine;
    private float _waitTime = 0.1f;

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

    public override void Attack()
    {
        AttackStrategy.Execute();
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