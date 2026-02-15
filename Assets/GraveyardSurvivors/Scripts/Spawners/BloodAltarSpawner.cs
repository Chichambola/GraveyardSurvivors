using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltarSpawner : Spawner<BloodAltar>
{
    [SerializeField] private Transform _point;

    public event Action<BloodAltar> AltarWasChosen;
    
    private void OnEnable()
    {
        Spawn();
    }

    public void Spawn()
    {
        GetObject();
    }
    
    protected override void ActionOnGet(BloodAltar altar)
    {
        altar.transform.position = _point.position;
        altar.transform.parent = transform;
        
        altar.WasChosen += OnAltarChosen;
        altar.CanBeReleased += Release;
        
        base.ActionOnGet(altar);
    }

    protected override void ActionOnRelease(BloodAltar altar)
    {
        altar.WasChosen -= OnAltarChosen;
        altar.CanBeReleased -= Release;
        
        base.ActionOnRelease(altar);
    }

    private void OnAltarChosen(BloodAltar altar)
    {
        AltarWasChosen?.Invoke(altar);
    }
}
