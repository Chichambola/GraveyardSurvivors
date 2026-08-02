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

public class RadiusEffectScaler : MonoBehaviour
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;

    private CancellationTokenSource _cts;
    private TweenSettings<float> _settings;
    private float _initialRadius;
    private float _targetRadius;
    private float _time;

    public float Value => _collider.radius;
    public float InitialValue => _initialRadius;

    private void Awake()
    {
        _initialRadius = _radius;
        
        _settings = new TweenSettings<float>();
    }

    private void OnEnable()
    {
        _collider.radius = _radius;
        _area.transform.localScale = new Vector3(_radius, _radius, _radius);
        
        _settings.settings.ease = Ease.Linear;
    }

    public void ResetToInitialValue() => ChangeRadius(_initialRadius).Forget();
    
    public void SetDuration(float duration) => ChangeRadius(_targetRadius, duration).Forget();
    
    public void StopChanging() => _cts?.Cancel();

    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
    }
    
    public async UniTask ChangeRadius(float targetRadius, float time = 0f)
    {
        CreateToken();
        
        _time = Mathf.Approximately(time, default) ? _rateWhenGainingEnergy : time;
        
        SetSettings(targetRadius);
        
        await Tween.Scale(_collider.transform, _settings)
            .Group(Tween.Scale(_area.transform, _settings))
            .ToYieldInstruction()
            .ToUniTask(cancellationToken: _cts.Token);
        
        _cts.Cancel();
    }
    
    private void CreateToken()
    {
        if(_cts is { IsCancellationRequested: false })
            return;
        
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
    }
    
    private void SetSettings(float targetRadius)
    {
        if (targetRadius > _initialRadius)
            _targetRadius = _initialRadius;
        
        _settings.startValue = _collider.radius;
        _settings.endValue = _targetRadius;
        _settings.settings.duration = _time;
    }
}
