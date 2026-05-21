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
    
    public void UpdateStats(float currentHealth, float maxHealth)
    {
        _text.text = $"Health: {currentHealth:f1} / {maxHealth} \n";
    }
}
