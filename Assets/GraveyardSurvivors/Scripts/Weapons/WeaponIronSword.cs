using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponIronSword : Weapon
{
    [SerializeField] private ParticleSystem _slash;
    [SerializeField] private AttackArea _area;
    
    private Coroutine _slashCoroutine;

    private void OnEnable()
    {
        if(_slashCoroutine != null)
            StopCoroutine(_slashCoroutine);

        _slashCoroutine = StartCoroutine(CountingParticles());
    }

    public override void Attack(float duration)
    {
        SetParticleSystemDuration(duration);
        
        _slash.Play();
    }

    private void SetParticleSystemDuration(float duration)
    {
        _slash.Stop();
        
        if (_slash.isPlaying)
            return;   
        
        var slashMain = _slash.main;

        slashMain.duration = duration;
    }

    private IEnumerator CountingParticles()
    {
        while (enabled)
        {
            _area.SetVisibility(_slash.particleCount > 0);

            yield return null;
        }
    }
}
