using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Sherbert.Framework.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SerializableDictionary<Player, PlayerInfo> _playerUpgradeStats;
    [SerializeField] private ExperienceHandler _experienceHandler;
    [SerializeField] private CinemachineVirtualCamera _playerCamera;
    [SerializeField] private UpgradesDisplayer _upgradesDisplayer;
    
    private Player _player;
    private CharacterStats _statsToUpgrade;

    private void OnEnable()
    {
        _experienceHandler.PlayerReachedThreshold += OnPlayerReachedThreshold;
        _upgradesDisplayer.UpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        _experienceHandler.PlayerReachedThreshold -= OnPlayerReachedThreshold;
        _player.GainedXp -= OnPlayerGainedXp;
    }

    public Player Spawn(Player player)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player));
        
        if (_playerUpgradeStats.TryGetValue(player, out PlayerInfo stat))
        {
            _statsToUpgrade = stat.GetStats();
            
            _player = Instantiate(player, _spawnPoint.position, _spawnPoint.rotation);
            _player.transform.parent = transform;
            _player.GainedXp += OnPlayerGainedXp;
            _playerCamera.Follow = _player.transform;

            return _player;
        }
        else
        {
            throw new KeyNotFoundException($"Player {player.name} has not been registered.");
        }
    }
    
    private void OnPlayerReachedThreshold()
    {
       _player.Upgrade(_statsToUpgrade);
       _upgradesDisplayer.ShowUpgrades();
    }
    
    private void OnPlayerGainedXp(float value)
    {
        float tempXp = _player.CurrentStats.XpMultiplier * value;

        tempXp = tempXp.RoundToFifths();
        
        _experienceHandler.GainExperience(tempXp);
    }
    
    private void OnUpgradeSelected(IItem item)
    {
        if (item is IBuff buff)
        {
            _player.AddBuff(buff);
        }

        if (item is IWeapon weapon)
        {
            _player.AddWeapon(weapon);
        }
    }
}
