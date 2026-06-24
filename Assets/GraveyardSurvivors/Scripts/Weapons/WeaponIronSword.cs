using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WeaponIronSword : WeaponWithAbility
{
    [SerializeField] private MeleeAttackStrategy _attackStrategy;
    [SerializeField] private AttackArea _area;
    [SerializeField] private ParticleSystem _attackParticles;
    [SerializeField] private KnockBack _knockBack;

    private Coroutine _attackingRoutine;
    private Coroutine _visibilityRoutine;
    private float _waitTime = 0.1f;
    private Vector3 _debugPos;

    public override string UpgradeDescription { get; protected set; }

    public override void Init()
    {
        UpgradeDescription = $"Add +{BonusDamagePerUpgrade} damage. \n" +
                             $"Add +{_attackStrategy.RadiusPercentGain}% to attack radius.\n" +
                             $"Add +{_knockBack.KnockBackPercentGain}% to knock back force;";
    }

    private void OnEnable()
    {
        _attackStrategy.AttackerDetected += OnAttackerDetected;
    }

    private void OnDisable()
    {
        _attackStrategy.AttackerDetected -= OnAttackerDetected;
    }

    public override void Upgrade()
    {
        _attackStrategy.Upgrade();
        
        _knockBack.Upgrade();
        
        IncreaseParticleSize();
        
        base.Upgrade();
    }

    public override void StartAttacking()
    {
        if (_attackingRoutine != null)
            StopCoroutine(_attackingRoutine);

        if (_visibilityRoutine != null)
            StopCoroutine(_visibilityRoutine);

        _visibilityRoutine = StartCoroutine(VisibilityRoutine());
        _attackingRoutine = StartCoroutine(AttackingRoutine());
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
        ProcessAttacker(attacker);

        _knockBack.Apply(attacker);
    }

    private IEnumerator AttackingRoutine()
    {
        var wait = new WaitForSeconds(Cooldown);
        
        while (enabled)
        {
            yield return wait;
            
            _attackStrategy.Execute();
            
            SetParticleSystemDuration(Cooldown);
        }
    }

    private IEnumerator VisibilityRoutine()
    {
        var wait = new WaitForSeconds(_waitTime);
        
        while (enabled)
        {
            yield return wait;
            
            _area.SetActive(_attackParticles.particleCount > 0);
        }
    }

    private void IncreaseParticleSize()
    {
        var xScale = _attackParticles.transform.localScale.x.AddPercentToNumber(_attackStrategy.RadiusPercentGain);
        var yScale = _attackParticles.transform.localScale.y.AddPercentToNumber(_attackStrategy.RadiusPercentGain);
        var zScale = _attackParticles.transform.localScale.z.AddPercentToNumber(_attackStrategy.RadiusPercentGain);

        _attackParticles.transform.localScale = new Vector3(xScale, yScale, zScale);
    }
}