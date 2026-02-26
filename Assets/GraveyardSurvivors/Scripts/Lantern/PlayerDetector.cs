using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : Detector
{
    public event Action<Player> PlayerDetected;
    public event Action PlayerLeft;
    public bool IsPlayerNear { get; private set; }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            PlayerDetected?.Invoke(player);

            IsPlayerNear = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            PlayerLeft?.Invoke();

            IsPlayerNear = false;
        }
    }
}
