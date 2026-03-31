using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolable<ParticleEffect>
{
    [SerializeField] private ParticleSystem _particleSystem;
    
    public event Action<ParticleEffect> CanBeReleased;
    
    public void ResetCharacteristics()
    {
        
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void SetDuration(float duration)
    {
        _particleSystem.Stop();
        
        if(_particleSystem.isPlaying)
            return;
        
        var systemMain = _particleSystem.main;

        systemMain.duration = duration;
    }

    public void SetPosition(Vector3 position) => _particleSystem.transform.position = position;
    
    public void StartPlaying() => _particleSystem.Play();
}
