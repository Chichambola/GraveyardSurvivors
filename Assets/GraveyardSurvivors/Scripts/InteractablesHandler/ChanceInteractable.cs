using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class ChanceInteractable<T> : Interactable
{
    [SerializeField] protected int CommonChancePercent;
    [SerializeField] protected int RareChancePercent;
    [SerializeField] protected int LegendaryChancePercent;
    [SerializeField] protected QuadraticCurvePoints Points;

    public abstract event Action<T> WasChosen; 
    
    public QuadraticCurvePoints CurrentPoints => Points;
    
    public int CommonChance => CommonChancePercent;
    public int RareChance => RareChancePercent;
    public int LegendaryChance => LegendaryChancePercent;
}
