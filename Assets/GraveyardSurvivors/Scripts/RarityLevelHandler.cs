using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityLevelHandler : MonoBehaviour
{
    [SerializeReference] private List<RarityLevel> _levels;

    public List<RarityLevel> Weights { get; private set; }

    private void Awake()
    {
        Weights = new List<RarityLevel>();
        
        foreach (var level in _levels)
        {
            Weights.Add(level);
        }
    }
}
