using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.Composites;
using Sequence = PrimeTween.Sequence;
using TimeoutController = Cysharp.Threading.Tasks.TimeoutController;

public class RadiusEffectScaler : MonoBehaviour
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private float _maxTimeScale = 2;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;

    private Coroutine _coroutine;
    private float _defaulTimeScale = 1;
    private float _initialRadius;
    private float _targetRadius;
    private float _time;

    public float Value => _collider.radius;
    public float InitialValue => _initialRadius;

    public void Init(float time)
    {
        _collider.radius = _radius;
        _area.transform.localScale = new Vector3(_radius, _radius, _radius);
    }
    
    private void Awake()
    {
        _initialRadius = _radius;
    }
    
    public void ResetToInitialValue() => ChangeRadius(_initialRadius).Forget();

    public void StopChanging() => _cts?.Cancel();

    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
    }
    
    public IEnumerator ChangeRadiusRoutine()
    {
        
        
    }
}
