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

    private CancellationToken _cts;
    private TweenSettings<float> _settings;
    private Sequence _sequence;
    private float _defaulTimeScale = 1;
    private float _initialRadius;
    private float _targetRadius;
    private float _time;
    private TimeoutController _timeoutController;

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
        
        _timeoutController = new TimeoutController();
        _settings = new TweenSettings<float>();
    }
    
    public void ResetToInitialValue() => ChangeRadius(_initialRadius).Forget();
    
    public void IncreaseSpeed(float value)
    {
        float gainPercent = _sequence.timeScale.GetClampedValue(value, _maxTimeScale);
        
        _sequence.timeScale += _sequence.timeScale.AddPercentToNumber(gainPercent);
    }

    public void DecreaseSpeed(float value)
    {
        float lostPercent = _sequence.timeScale.GetClampedValueInverse(value, _defaulTimeScale);
        
        _sequence.timeScale -= _sequence.timeScale.SubtractPercentFromNumber(lostPercent);
    }

    public void StopChanging() => _cts?.Cancel();

    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
    }
    
    public async UniTask ChangeRadius(float targetRadius, float time = 0f)
    {
        _targetRadius = targetRadius;
        
        _time = Mathf.Approximately(time, default) ? _rateWhenGainingEnergy : time;
        
        try
        {
            _cts = _timeoutController.Timeout(TimeSpan.FromSeconds(_time));

            await _sequence;
            
            _timeoutController.Reset();
        }
        catch (Exception ex)
        {
            if (_timeoutController.IsTimeout())
            {
                Debug.LogError("Операция не уложилась в отведенное время!");
            }
            else
            {
                Debug.LogException(ex);
            }
        }
    }
}
