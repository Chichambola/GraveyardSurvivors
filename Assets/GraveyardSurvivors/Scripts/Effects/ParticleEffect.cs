using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolable<ParticleEffect>
{
    public event Action<ParticleEffect> CanBeReleased;
    
    public void ResetCharacteristics()
    {
        throw new NotImplementedException();
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
