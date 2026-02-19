using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class ChanceInteractable<T> : Interactable
{
    [Header("Chances")]
    [SerializeField] protected int CommonChancePercent;
    [SerializeField] protected int RareChancePercent;
    [SerializeField] protected int LegendaryChancePercent;
    [Header("Quadratic curve points")]
    [SerializeField] private Transform _aPoint;
    [SerializeField] private Transform _bPoint;
    [SerializeField] private Transform _controlPoint;
    [SerializeField] private QuadraticCurvePoints _points;

    public abstract event Action<T> WasChosen; 
    
    public QuadraticCurvePoints CurrentPoints => _points;
    
    public int CommonChance => CommonChancePercent;
    public int RareChance => RareChancePercent;
    public int LegendaryChance => LegendaryChancePercent;

    private void OnEnable()
    {
        _points.SetPositions(_aPoint, _bPoint, _controlPoint);
    }
}
