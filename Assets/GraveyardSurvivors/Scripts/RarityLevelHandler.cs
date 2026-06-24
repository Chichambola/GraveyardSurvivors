using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RarityLevelHandler : MonoBehaviour
{
    [SerializeField] private RarityLevel _common;
    [SerializeField] private RarityLevel _rare;
    [SerializeField] private RarityLevel _legendary;

    public List<RarityLevel> Weights { get; private set; }

    private void Awake()
    {
        Weights = new () {_common, _rare, _legendary};
    }
}
