using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour, IPoolable<ParticleEffect>
{
    [SerializeField] private ParticleSystem _particleSystem;
    
    public event Action<ParticleEffect> CanBeReleased;
    private float _duration;
    private Coroutine _coroutine;
    
    public void ResetCharacteristics()
    {
        _particleSystem.transform.position = Vector3.zero;
        
        _duration = 0;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);
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
        
        _duration = duration;
    }

    public void SetRadius(float radius)
    {
        float offset = 2f;
        
        _particleSystem.Stop();
        
        if(_particleSystem.isPlaying)
            return;
        
        var systemShape = _particleSystem.shape;

        systemShape.radius = radius / offset;
    }
    
    public void StartPlaying()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(DurationRoutine());
        
        _particleSystem.Play();
    }

    private IEnumerator DurationRoutine()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime <= _duration)
        {
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _particleSystem.Stop();
        
        Release();
    }
}
