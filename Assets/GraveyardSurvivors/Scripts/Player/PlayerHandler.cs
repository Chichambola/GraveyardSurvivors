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
    [SerializeField] private UpgradesHandler _upgradesDisplayer;

    private Player _player;
    private CharacterStats _statsToUpgrade;
    private float _normalTimeSpeed = 1;
    private float _pauseTime = 0.00001f;

    private void OnEnable()
    {
        _experienceHandler.PlayerReachedThreshold += OnPlayerReachedThreshold;
        _upgradesDisplayer.ItemSelected += OnItemSelected;
        _upgradesDisplayer.WeaponSelected += OnWeaponSelected;
    }

    private void OnDisable()
    {
        _experienceHandler.PlayerReachedThreshold -= OnPlayerReachedThreshold;
        _upgradesDisplayer.ItemSelected -= OnItemSelected;
        _upgradesDisplayer.WeaponSelected -= OnWeaponSelected;
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
            _playerCamera.Follow = _player.transform;

            _player.GainedXp += OnPlayerGainedXp;

            _upgradesDisplayer.SetPlayer(_player);

            return _player;
        }
        else
        {
            throw new KeyNotFoundException($"Player {player.name} has not been registered.");
        }
    }

    private void OnPlayerReachedThreshold()
    {
        Time.timeScale = _pauseTime;
        _upgradesDisplayer.ShowUpgrades();
        _player.Upgrade(_statsToUpgrade);
    }

    private void OnPlayerGainedXp(float value)
    {
        float tempXp = _player.CurrentStats.XpMultiplier * value;

        tempXp = tempXp.RoundToFifths();

        _experienceHandler.GainExperience(tempXp);
    }

    private void OnItemSelected(Item item)
    {
        Time.timeScale = _normalTimeSpeed;

        if (item is IBuff buff)
        {
            _player.AddBuff(buff);
        }
    }

    private void OnWeaponSelected(Weapon upgrade)
    {
        Time.timeScale = _normalTimeSpeed;
        
        if (_player.HasWeapon(upgrade))
        {
            _player.UpgradeWeapon(upgrade);
        }
        else
        {
            _player.AddWeapon(upgrade);
        }
    }
}