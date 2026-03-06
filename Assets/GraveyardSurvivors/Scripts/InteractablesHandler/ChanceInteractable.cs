using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class ChanceInteractable<T> : Interactable
{
    [Header("Weights")]
    [SerializeField] protected int CommonChanceWeight;
    [SerializeField] protected int RareChanceWeight;
    [SerializeField] protected int LegendaryChanceWeight;

    public abstract event Action<T> WasChosen; 
    
    public int CommonChance => CommonChanceWeight;
    public int RareChance => RareChanceWeight;
    public int LegendaryChance => LegendaryChanceWeight;
}
