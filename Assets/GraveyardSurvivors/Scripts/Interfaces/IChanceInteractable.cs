using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChanceInteractable
{
    public List<RarityLevel> Weights { get; }
}
