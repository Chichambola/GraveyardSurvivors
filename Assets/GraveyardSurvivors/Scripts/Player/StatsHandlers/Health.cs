using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldown;
    
    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Healing());
    }

    private IEnumerator Healing()
    {
        var wait = new WaitForSecondsRealtime(_cooldown);

        while (enabled)
        {
            yield return wait;
            
            _player.Heal(_player.CurrentStats.HealthRegeneration);
        }
    }
}
