using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldown;
    [Header("Services")]
    [SerializeField] private Defender _defender;
    [SerializeField] private Evader _evader;
    [SerializeField] private StatsViewer _statsViewer;
    
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

    public void UpdateStats()
    {
        _statsViewer.UpdateStats(_player.CurrentHealth, _player.CurrentStats.MaxHealth);   
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
            damage = DetermineDamageAmount(damage);
            
            return true;
        }
    }
    
    private float DetermineDamageAmount(float damage)
    {
        if (_defender.TryBlockDamage(_player.CurrentStats.BlockChance, _player.CurrentStats.Luck, ref damage))
        {
            Debug.Log("Blocked");
        }
        
        damage = damage.AddPercentToNumber(_player.CurrentStats.IncomingDamageMultiplier);
        
        return damage;
    }
}
