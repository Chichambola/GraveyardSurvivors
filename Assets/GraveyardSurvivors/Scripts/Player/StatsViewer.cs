using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class StatsViewer : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _player.StatsChanged += OnStatsChanged;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= OnStatsChanged;
    }
    
    private void OnStatsChanged(CharacterStats stats)
    {
        _text.text = $"Health: {stats.Health:f1} \n" +
                     $"Health regeneration: {stats.HealthRegeneration:f1}\n" +
                     $"Armor: {stats.Armor:f1}\n" +
                     $"MS: {stats.MovementSpeed:f1}\n" +
                     $"Attack speed: {stats.AttackSpeed:f1}\n" +
                     $"Attack radius: {stats.AttackRadius:f1}\n" +
                     $"Pickup radius: {stats.PickUpRadius:f1}\n" +
                     $"Block chance: {stats.BlockChance:f1}\n" +
                     $"Evasion chance: {stats.EvasionChance:f1}\n" +
                     $"Crit chance: {stats.CritChance:f1}\n" +
                     $"Crit multiplier: {stats.CritMultiplier:f1}\n" +
                     $"XP Multiplier: {stats.XpMultiplier:f1}\n" +
                     $"Gold multiplier: {stats.GoldMultiplier:f1}\n" +
                     $"Luck: {stats.Luck:f1}";
    }
}
