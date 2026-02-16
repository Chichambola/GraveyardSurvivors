using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceAltarSpawner : Spawner<ChanceAltar>
{
    [SerializeField] private Transform _point;

    public event Action<ChanceAltar> AltarWasChosen;
    
    private void OnEnable()
    {
        Spawn();
    }

    public void Spawn()
    {
        GetObject();
    }
    
    protected override void ActionOnGet(ChanceAltar altar)
    {
        ActiveObjects.Add(altar);
        
        altar.transform.position = _point.position;

        altar.CanBeReleased += Release;
        altar.WasChosen += OnAltarChosen;
        
        base.ActionOnGet(altar);
    }

    protected override void ActionOnRelease(ChanceAltar altar)
    {
        altar.CanBeReleased -= Release;
        altar.WasChosen -= OnAltarChosen;
        
        base.ActionOnRelease(altar);
        
        ActiveObjects.Remove(altar);
    }

    private void OnAltarChosen(ChanceAltar altar)
    {
        AltarWasChosen?.Invoke(altar);
    }
}
