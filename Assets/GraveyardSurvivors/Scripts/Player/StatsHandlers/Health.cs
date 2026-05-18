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

    public bool TryTakeDamage(float damage, out bool hasTakenDamage)
    {
        if (_evader.CanEvade(_player.CurrentStats.EvasionChance, _player.CurrentStats.Luck))
        {
            Debug.Log("Evaded");

            hasTakenDamage = false;
            
            return hasTakenDamage;
        }
        else
        {
            damage = DetermineDamageAmount(damage);
            
            _player.CurrentStats.Health -= damage;

            hasTakenDamage = true;
            
            return hasTakenDamage;
        }
    }
    
    private float DetermineDamageAmount(float damage)
    {
        if (_defender.CanBlock(_player.CurrentStats.BlockChance, _player.CurrentStats.Luck))
        {
            Debug.Log("Blocked");
            
            damage = _defender.GetBlockedDamage(damage);
        }
        
        damage = damage.AddPercentToNumber(_player.CurrentStats.IncomingDamageMultiplier);
        
        damage = _defender.GetDamageAmount(_player.CurrentStats.Armor, damage);
        
        return damage;
    }
}
