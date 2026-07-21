using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _healCooldown;
    [SerializeField] private float _reduceDamageTime = 1f;
    [SerializeField] private float _waitTime = 1;
    [Header("Services")] 
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private StatsViewer _statsViewer;

    private Coroutine _healthRegenerationRoutine;
    private Coroutine _reduceDamageRoutine; 
    private int _damageReduceAfterDamage = 80;
    private bool _isTakingLessDamage;
    private bool _isOnCooldown;

    private void OnEnable()
    {
        _isTakingLessDamage = false;
        _isOnCooldown = false;

        if (_healthRegenerationRoutine != null)
            StopCoroutine(_healthRegenerationRoutine);

        _healthRegenerationRoutine = StartCoroutine(Healing());
    }

    public void UpdateStats() => _statsViewer.UpdateStats(_player.CurrentHealth, _player.CurrentStats.MaxHealth);

    private IEnumerator ReducedDamageRoutine()
    {
        var wait = new WaitForSeconds(_reduceDamageTime);
        
        while (enabled)
        {
            _isTakingLessDamage = true;
            
            yield return wait;
            
            wait = new WaitForSeconds(_waitTime);
            
            _isTakingLessDamage = false;
            
            _isOnCooldown = true;
            
            yield return wait;
            
            _isOnCooldown = false;
        }
    }
    
    private IEnumerator Healing()
    {
        var wait = new WaitForSeconds(_healCooldown);

        while (enabled)
        {
            yield return wait;

            _player.Heal(_player.CurrentStats.HealthRegeneration);

            UpdateStats();
        }
    }

    public bool TryTakeDamage(ref float damage)
    {
        if (_evader.CanEvade(_player.CurrentStats.EvasionChance, _player.CurrentStats.Luck))
        {
            Debug.Log("Evaded");

            return false;
        }

        if (_defender.TryBlockDamage(_player.CurrentStats.BlockChance, _player.CurrentStats.Luck, ref damage))
        {
            Debug.Log("Blocked");
        }

        damage = damage.AddPercentToNumber(_player.CurrentStats.IncomingDamageMultiplier);
        
        if (_isTakingLessDamage)
            damage = damage.SubtractPercentFromNumber(_damageReduceAfterDamage);

        if (_isTakingLessDamage || _isOnCooldown) 
            return true;
        
        if (_reduceDamageRoutine != null)
            StopCoroutine(_reduceDamageRoutine);
                
        _reduceDamageRoutine = StartCoroutine(ReducedDamageRoutine());

        return true;
    }
}