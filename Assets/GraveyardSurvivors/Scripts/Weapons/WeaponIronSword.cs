using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponIronSword : Weapon
{
    [SerializeField] private AttackArea _area;
    [SerializeField] private float _waitTime = 0.1f;
    [SerializeField] private ParticleSystem _slash;
    
    private Coroutine _slashCoroutine;
    
    private void OnEnable()
    {
        if(_slashCoroutine != null)
            StopCoroutine(_slashCoroutine);

        _slashCoroutine = StartCoroutine(VisibilityCoroutine());
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

    private IEnumerator VisibilityCoroutine()
    {
        var wait = new WaitForSeconds(_waitTime);
        
        while (enabled)
        {
            yield return wait;
            
            _area.SetVisibility(_slash.particleCount > 0);
        }
    }
}
