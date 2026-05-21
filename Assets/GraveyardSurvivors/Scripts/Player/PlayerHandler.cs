using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private CharacterStats _statsToUpgrade;
    [SerializeField] private ExperienceHandler _experienceHandler;

    private void Awake()
    {
        _statsToUpgrade = new CharacterStats(_statsToUpgrade);
    }

    private void OnEnable()
    {
        _experienceHandler.PlayerReachedThreshold += OnPlayerReachedThreshold;
        _player.GainedXp += OnPlayerGainedXp;
    }

    private void OnDisable()
    {
        _experienceHandler.PlayerReachedThreshold -= OnPlayerReachedThreshold;
        _player.GainedXp -= OnPlayerGainedXp;
    }
    
    private void OnPlayerReachedThreshold()
    {
        _player.Upgrade(_statsToUpgrade);
    }
    
    private void OnPlayerGainedXp(float value)
    {
        float tempXp = _player.CurrentStats.XpMultiplier * value;

        tempXp = tempXp.RoundToTenths();
        
        _experienceHandler.GainExperience(tempXp);
    }
}
