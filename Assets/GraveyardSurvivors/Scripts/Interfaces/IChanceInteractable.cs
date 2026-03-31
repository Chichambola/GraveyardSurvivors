using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChanceInteractable
{
    public int CommonChance { get; }
    public int RareChance { get; }
    public int LegendaryChance { get; }
}
