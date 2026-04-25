using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RarityLevel : MonoBehaviour, IWeightedObject
{
    [SerializeField] private int _weight;
    [SerializeField] private ERarityLevel _rarity;

    private int _initialWeight;
    
    public int Weight => _weight;
    public ERarityLevel Rarity => _rarity;

    private void Awake()
    {
        _initialWeight = _weight;
    }

    public void ResetChance()
    {
        if (_weight != _initialWeight)
        {
            _weight = _initialWeight;   
        }
    }

    public void IncreaseWeight(float multiplier)
    {
        _weight = _weight.AddPercentToNumber(multiplier);
    }
}
