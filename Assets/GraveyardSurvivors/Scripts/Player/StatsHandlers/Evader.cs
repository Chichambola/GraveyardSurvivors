using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : Stats<CharacterStats>
{
    [SerializeField] private Player _player;
    
    private float _evasionPercent;

    public bool CanEvade(float luck)
    {
        float currentEvasionChance = _evasionPercent + luck;
        
        int randomPercent = Random.Range(UserUtils.LowestPercent, UserUtils.HighestPercent);

        return !(randomPercent > currentEvasionChance);
    }
    
    public override void UpdateStats(CharacterStats stats)
    {
        _evasionPercent = stats.EvasionChance;
    }
}
