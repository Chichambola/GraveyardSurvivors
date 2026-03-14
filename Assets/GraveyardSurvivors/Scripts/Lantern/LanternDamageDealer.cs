using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternDamageDealer : MonoBehaviour
{
    [SerializeField] private float _damage = 3f;
    [SerializeField] private float _rate = 1f;
    
    private List<Enemy> _enemiesInRange;
    private Coroutine _coroutine;

    private void Awake()
    {
        _enemiesInRange = new List<Enemy>();
    }

    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DamageRoutine());
    }

    public void UpdateEnemies(List<Enemy> enemies)
    {
        _enemiesInRange = enemies;
    }
    
    private IEnumerator DamageRoutine()
    {
        var wait = new WaitForSecondsRealtime(_rate);
        
        while (enabled)
        {
            if (_enemiesInRange.Count > 0)
            {
                foreach (var enemy in _enemiesInRange)
                {
                    enemy.TakeDamage(_damage);
                }
            }
            
            yield return wait;
        }
    }
}
