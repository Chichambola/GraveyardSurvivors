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
        _text.text = $"Health: {stats.Health} \n" +
                     $"Health regeneration: {stats.HealthRegeneration}\n" +
                     $"Armor: {stats.Armor}\n" +
                     $"MS: {stats.MovementSpeed}\n" +
                     $"Attack speed: {stats.AttackSpeed}\n" +
                     $"Attack radius: {stats.AttackRadius}\n" +
                     $"Pickup radius: {stats.PickUpRadius}\n" +
                     $"Block chance: {stats.BlockChance}\n" +
                     $"Evasion chance: {stats.EvasionChance}\n" +
                     $"Crit chance: {stats.CritChance}\n" +
                     $"Crit multiplier: {stats.CritMultiplier}\n" +
                     $"XP Multiplier: {stats.XpMultiplier}\n" +
                     $"Gold multiplier: {stats.GoldMultiplier}\n" +
                     $"Luck: {stats.Luck}";
    }
}
