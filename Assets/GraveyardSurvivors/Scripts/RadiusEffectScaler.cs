
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using PrimeTween;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.ProBuilder.Shapes;
using Sequence = PrimeTween.Sequence;
using Tween = PrimeTween.Tween;

public class RadiusEffectScaler : MonoBehaviour
{
    [SerializeField] private bool _isTurnedOn;
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private float _maxTimeScale = 2;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Ease _easeType;

    public event Action ThresholdReached;
    
    private CancellationTokenSource _cts;
    private Sequence _sequence;
    private TweenSettings<float> _settings;
    private float _defaulTimeScale = 1;
    private float _initialRadius;
    private float _currentTimeScale;
    private float _targetRadius;
    private float _time;
    private float _disableThreshold;

    public float Value => _collider.radius;
    public float InitialValue => _initialRadius;
    
    private void Awake()
    {
        _initialRadius = _radius;
        _settings = new TweenSettings<float>();
        
        var radius = new Vector3(_radius, _radius, _radius);
        
        _collider.transform.localScale = radius;
        _area.transform.localScale = radius;
        _settings.settings.ease = _easeType;
        _collider.radius = 1;
        _currentTimeScale = 1;
    }
    
    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
        _cts?.Cancel();
    }
    
    public void IncreaseSpeed(float duration)
    {
        return;
        
        var gainPercent = _sequence.timeScale.GetClampedValue(duration, _maxTimeScale);
        
        _sequence.timeScale = _sequence.timeScale.AddPercentToNumber(gainPercent);

        _currentTimeScale = _sequence.timeScale;
    }
    
    public void DecreaseSpeed(float duration)
    {
        return;
        
        var lostPercent = _sequence.timeScale.GetClampedValueInverse(duration, _defaulTimeScale);
        
        _sequence.timeScale = _sequence.timeScale.SubtractPercentFromNumber(lostPercent);

        _currentTimeScale = _sequence.timeScale;
    }

    public void StopChanging() => _cts?.Cancel();

    public async UniTask ChangeRadius(float targetRadius, float time = 0f, float disableThreshold = 0f)
    {
        CreateToken();
        
        _time = Mathf.Approximately(time, default) ? _rateWhenGainingEnergy : time;

        SetSettings(targetRadius);
        
        _sequence = Sequence.Create()
            .Group(Tween.Scale(_collider.transform, _settings)
                .Group(Tween.Scale(_area.transform, _settings)));
        
        _sequence.timeScale = _currentTimeScale;
        
        if (!Mathf.Approximately(disableThreshold, default))
        {
            _disableThreshold = disableThreshold;
            _sequence.Group(Tween.Custom(_collider, _settings, OnValueChange));
        }
        
        await _sequence.ToYieldInstruction().WithCancellation(_cts.Token);
        
        _cts.Cancel();
    }

    private void OnValueChange(SphereCollider sphere, float value)
    {
        Debug.Log("Here");
        
        if (Value < _disableThreshold)
        {
            ThresholdReached?.Invoke();
        }
    }

    private void CreateToken()
    {
        if (_cts is { IsCancellationRequested: false })
            _cts.Cancel();
        
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
    }

    private void SetSettings(float targetRadius)
    {
        _targetRadius = targetRadius;
        
        _settings.startValue = _collider.radius;
        _settings.endValue = _targetRadius;
        _settings.settings.duration = _time;
    }
}
