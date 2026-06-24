using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _healCooldown;
    [SerializeField] private float _invincibilityAfterDamage = .30f;
    [Header("Services")]
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private StatsViewer _statsViewer;
    
    private Coroutine _coroutine;
    private IntervalTimer _timer;
    
    public bool CanTakeDamage { get; private set; }
    
    private void OnEnable()
    {
        CanTakeDamage = true;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Healing());
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
        else
        {
            if (_defender.TryBlockDamage(_player.CurrentStats.BlockChance, _player.CurrentStats.Luck, ref damage))
            {
                Debug.Log("Blocked");
            }
            
            damage = damage.AddPercentToNumber(_player.CurrentStats.IncomingDamageMultiplier);
            
            _timer = new IntervalTimer(_invincibilityAfterDamage);
            _timer.Stopped += OnDamageTimerStopped;
            _timer.Start();
            
            CanTakeDamage = false;
            
            return true;
        }
    }
    
    public void UpdateStats()
    {
        _statsViewer.UpdateStats(_player.CurrentHealth, _player.CurrentStats.MaxHealth);   
    }
    
    private void OnDamageTimerStopped()
    {
        CanTakeDamage = true;
        
        _timer.Stopped -= OnDamageTimerStopped;
        
        _timer?.Stop();
    }
}
