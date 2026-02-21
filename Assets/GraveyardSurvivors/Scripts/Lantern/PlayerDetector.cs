using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : Detector
{
    public event Action<IBuffable> PlayerDetected;
    public event Action PlayerLeft;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IBuffable player))
        {
            Debug.Log("Player detected");
            
            PlayerDetected?.Invoke(player);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            PlayerLeft?.Invoke();
        }
    }
}
