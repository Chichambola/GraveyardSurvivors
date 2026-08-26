using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Interactable
{
    [SerializeField] private float _timeBeforeRelease = 2f;
    
    private IntervalTimer _timer;
    
    public Vector3 CurrentPosition => transform.position;
    
    public override void ProcessInteraction()
    {
        base.ProcessInteraction();

        IsAvailable = false;

        _timer = new IntervalTimer(_timeBeforeRelease);
        _timer.Stopped += Release;
        _timer.Start();
    }

    public override void Release()
    {
        _timer.Stopped -= Release;
        
        base.Release();
    }
}
