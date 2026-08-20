using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Interactable
{
    public Vector3 CurrentPosition => transform.position;
    
    public override void ProcessInteraction()
    {
        base.ProcessInteraction();

        IsAvailable = false;
    }
}
