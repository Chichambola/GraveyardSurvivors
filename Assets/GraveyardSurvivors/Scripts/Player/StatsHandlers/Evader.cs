using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : Stats<CharacterStats>
{
    [SerializeField] private Player _player;
    
    private float _evasionPercent;
    
    protected override void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    protected override void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }

    public bool CanEvade(float luck)
    {
        float currentEvasionChance = _evasionPercent + luck;
        
        int randomPercent = Random.Range(UserUtils.LowestPercent, UserUtils.HighestPercent);

        return !(randomPercent > currentEvasionChance);
    }
    
    protected override void OnStatsChanged(CharacterStats stats)
    {
        _evasionPercent = stats.EvasionChance;
    }

    public override void SetInitialStats(CharacterStats stats)
    {
        _evasionPercent = stats.EvasionChance;
    }
}
