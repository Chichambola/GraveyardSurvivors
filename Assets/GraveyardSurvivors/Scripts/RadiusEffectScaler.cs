
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Sequence = PrimeTween.Sequence;
using Tween = PrimeTween.Tween;

public class RadiusEffectScaler : MonoBehaviour
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _rateWhenGainingEnergy = 1f;
    [SerializeField] private float _maxTimeScale = 2;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;

    public event Action ThreshholdReached;
    
    private CancellationTokenSource _cts;
    private Sequence _sequence;
    private TweenSettings<float> _settings;
    private float _defaulTimeScale = 1;
    private float _initialRadius;
    private float _currentTimeScale;
    private float _targetRadius;
    private float _time;

    public float Value => _collider.radius;
    public float InitialValue => _initialRadius;

    public void Init()
    {
        _collider.radius = _radius;
        _area.transform.localScale = new Vector3(_radius, _radius, _radius);
        _settings.settings.ease = Ease.OutSine;
        _currentTimeScale = 1;
    }
    
    private void Awake()
    {
        _initialRadius = _radius;
        _settings = new TweenSettings<float>();
    }
    
    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
        _cts?.Cancel();
    }
    
    public void IncreaseSpeed(float duration)
    {
        var gainPercent = _sequence.timeScale.GetClampedValue(duration, _maxTimeScale);
        
        _sequence.timeScale = _sequence.timeScale.AddPercentToNumber(gainPercent);

        _currentTimeScale = _sequence.timeScale;
    }
    
    public void DecreaseSpeed(float duration)
    {
        var lostPercent = _sequence.timeScale.GetClampedValueInverse(duration, _defaulTimeScale);
        
        _sequence.timeScale = _sequence.timeScale.SubtractPercentFromNumber(lostPercent);

        _currentTimeScale = _sequence.timeScale;
    }

    public async UniTask ChangeRadius(float targetRadius, float time = 0f)
    {
        CreateToken();

        _time = Mathf.Approximately(time, default) ? _rateWhenGainingEnergy : time;

        SetSettings(targetRadius);

        _sequence = Sequence.Create()
            .Group(Tween.Scale(_collider.transform, _settings)
                .Group(Tween.Scale(_area.transform, _settings)));
        
        _sequence.timeScale = _currentTimeScale;
        
        await _sequence.ToYieldInstruction().WithCancellation(_cts.Token);

        ThreshholdReached?.Invoke();
        
        _cts.Cancel();
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
