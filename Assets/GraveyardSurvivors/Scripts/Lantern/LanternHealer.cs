using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternHealer : MonoBehaviour
{
    [SerializeField] private PlayerDetector _detector;
    [SerializeField] private LanternHealBuff _buff;
    [SerializeField] private float _cooldown = 1.5f;
    
    private IBuffable _buffable;
    private float _initialHealthRegeneration;
    private Coroutine _coroutine;
    private List<IBuff> _tempBuffs;

    private void OnEnable()
    {
        _detector.PlayerDetected += StartHealing;
        _detector.PlayerLeft += StopHealing;
        _tempBuffs = new List<IBuff>();
    }

    private void OnDisable()
    {
        _detector.PlayerDetected -= StartHealing;
        _detector.PlayerLeft -= StopHealing;
    }

    private void StartHealing(IBuffable buffable)
    {
        _buffable = buffable;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(HealingCoroutine());
    }

    private void StopHealing()
    {
        foreach (IBuff buff in _tempBuffs)
        {
            _buffable.RemoveBuff(buff);
        }
        
        _tempBuffs.Clear();
        
        StopCoroutine(_coroutine);
    }
    
    private IEnumerator HealingCoroutine()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            _tempBuffs.Add(_buff);
            _buffable.AddBuff(_buff);
            
            yield return wait;
        }
    }
}
