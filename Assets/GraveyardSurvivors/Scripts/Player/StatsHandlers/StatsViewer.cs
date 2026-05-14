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
        _player.StatsChanged += UpdateStats;
    }

    private void OnDisable()
    {
        _player.StatsChanged -= UpdateStats;
    }
    
    public void UpdateStats(CharacterStats stats)
    {
        _text.text = $"Health: {stats.Health:f1} \n";
    }
}
