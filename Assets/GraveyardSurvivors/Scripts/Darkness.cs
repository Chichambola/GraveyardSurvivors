using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageMultiplier = 0.5f;
    [SerializeField] private float _rate = 1.5f;

    private float _initialDamage;
    private IPlayer _player;
    private Coroutine _coroutine;
    private bool _isPlayerInDarkness;
    
    public void Init(IPlayer player)
    {
        _player = player ?? throw new Exception("Player can not be null");

        _initialDamage = _damage;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        var wait = new WaitForSeconds(_rate);
        
        while (enabled)
        {
            if (!_player.IsLightActive)
            {
                _player.TakeDamage(_damage);
                
                _damage +=  _damageMultiplier * _damage;
                
                yield return wait;
            }
            else
            {
                if (!Mathf.Approximately(_damage, _initialDamage))
                {
                    _damage = _initialDamage;
                }
                
                yield return null;   
            }
        }
    }
}
