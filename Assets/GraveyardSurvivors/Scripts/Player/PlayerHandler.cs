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

    private static Player _player;
    private static Attacker _playerAttacker;
    private CharacterStats _statsToUpgrade;

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
            _playerAttacker = _player.Attacker;
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

    public static bool HasPlayerWeapon(Weapon weapon) => _playerAttacker.HasWeapon(weapon);
    
    public static void AddEffect(Effect effect)
    {
        _playerAttacker.AddEffect(effect);
    }
    
    private void OnPlayerReachedThreshold()
    {
        Game.Pause();
        
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
        Game.Resume();
        
        
        if (item is IBuff buff)
        {
            _player.AddBuff(buff);
        }
        
        _player.ProcessItem(item);
    }

    private void OnWeaponSelected(Weapon weapon)
    {
        Game.Resume();
        
        if (HasPlayerWeapon(weapon))
        {
            _playerAttacker.UpgradeWeapon(weapon);
        }
        else
        {
            _playerAttacker.AddWeapon(weapon);
        }
    }
}