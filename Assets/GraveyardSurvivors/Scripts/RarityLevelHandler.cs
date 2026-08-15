using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityLevelHandler : MonoBehaviour
{
    [SerializeReference] private RarityLevel _none;
    [SerializeReference] private RarityLevel _common;
    [SerializeReference] private RarityLevel _rare;
    [SerializeReference] private RarityLevel _legendary;

    public List<RarityLevel> Weights { get; private set; }

    private void Awake()
    {
        Weights = new () {_none, _common, _rare, _legendary};
    }
}
