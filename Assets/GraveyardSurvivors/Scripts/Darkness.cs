using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Darkness : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _rate = 1.5f;

    private IPlayer _player;
    private Coroutine _coroutine;
    private bool _isPlayerInDarkness;
    
    public void Init(IPlayer player)
    {
        _player = player ?? throw new Exception("Player can not be null");
        
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
                
                yield return wait;
            }

            yield return null;
        }
    }
}
