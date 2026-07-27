using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerDetector : Detector
{
    public event Action<IPlayer> PlayerDetected;
    public event Action PlayerLeft;
    
    public bool IsPlayerNear { get; private set; }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player))
        {
            PlayerDetected?.Invoke(player);

            IsPlayerNear = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayer _))
        {
            PlayerLeft?.Invoke();

            IsPlayerNear = false;
        }
    }

    private void OnDisable()
    {
        IsPlayerNear = false;
    }
}
