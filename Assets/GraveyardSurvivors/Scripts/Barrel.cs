using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Interactable
{
    [SerializeField] private float _timeBeforeRelease = 2f;
    [Header("Mesh")] 
    [SerializeField] private GameObject _wholeMesh;
    [SerializeField] private GameObject _brokenMesh;
    
    private IntervalTimer _timer;
    
    public Vector3 CurrentPosition => transform.position;

    private void OnEnable()
    {
        ChangeMeshState(true);
    }

    public override void ProcessInteraction()
    {
        base.ProcessInteraction();

        IsAvailable = false;
        
        ChangeMeshState(false);

        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.Stopped += Release;
        _timer.Start();
    }

    public override void Release()
    {
        _timer.Stopped -= Release;
        
        base.Release();
    }

    private void ChangeMeshState(bool value)
    {
        _wholeMesh.SetActive(value);
        _brokenMesh.SetActive(!value);
    }
}
