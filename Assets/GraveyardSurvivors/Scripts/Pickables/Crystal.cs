using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Pickable
{
    public override event Action<Pickable> CanBeReleased;
    

    public override void ResetCharacteristics()
    {
        
    }

    public override void Release()
    {
        base.Release();
        
        CanBeReleased?.Invoke(this);
    }
}
