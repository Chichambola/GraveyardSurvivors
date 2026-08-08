
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using Sequence = PrimeTween.Sequence;
using Tween = PrimeTween.Tween;

public class RadiusEffectScaler : MonoBehaviour
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _maxTimeScale = 2;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private ThresholdVerifier _thresholdVerifier;
    [SerializeField] private Ease _easeType = Ease.InCubic;

    public event Action Reached;
    public event Action ThresholdReached;

    private Sequence _changeRadius;
    private CancellationTokenSource _cts;
    private TweenSettings<float> _settings;
    private float _initialRadius;
    private float _currentTimeScale;
    private float _targetRadius;
    private float _time;
    private readonly float _defaultTimeScale = 1;

    public float Value => _collider.transform.localScale.x;
    public float InitialValue => _initialRadius;
    
    private void Awake()
    {
        _initialRadius = _radius;
        _settings = new TweenSettings<float>();
        
        _settings.settings.ease = _easeType;
        
        _collider.radius = 1;
        _currentTimeScale = 1;
    }

    private void OnEnable()
    {
        _thresholdVerifier.ThresholdReached += OnThresholdReached;
    }

    private void OnDisable()
    {
        _thresholdVerifier.ThresholdReached -= OnThresholdReached;
    }

    private void OnValidate()
    {
        var radius = new Vector3(_radius, _radius, _radius);
        
        _collider.transform.localScale = radius;
        _area.transform.localScale = radius;
    }

    public void SetActive(bool value)
    {
        _collider.gameObject.SetActive(value);
        _area.gameObject.SetActive(value);
    }
    
    public void IncreaseSpeed(float duration)
    {
        return;
        
        var gainPercent = _changeRadius.timeScale.GetClampedValue(duration, _maxTimeScale);
        
        _changeRadius.timeScale = _changeRadius.timeScale.AddPercentToNumber(gainPercent);

        _currentTimeScale = _changeRadius.timeScale;
    }
    
    public void DecreaseSpeed(float duration)
    {
        return;
        
        var lostPercent = _changeRadius.timeScale.GetClampedValueInverse(duration, _defaultTimeScale);
        
        _changeRadius.timeScale = _changeRadius.timeScale.SubtractPercentFromNumber(lostPercent);

        _currentTimeScale = _changeRadius.timeScale;
    }

    public void StopChanging()
    {
        if (!_changeRadius.isAlive)
            return;
        
        _changeRadius.Stop();
        _cts.Cancel();
    }

    public void ChangeRadius(float targetRadius, float time = 0f, float disableThreshold = 0f)
    {
        StopChanging();

        if (!Mathf.Approximately(disableThreshold, default))
            _thresholdVerifier.Execute(disableThreshold);
        
        SetSettings(targetRadius, time);
        
        ChangeRadius().Forget();
    }

    private async UniTask ChangeRadius()
    {
        _cts = new CancellationTokenSource();
        _cts.RegisterRaiseCancelOnDestroy(gameObject);
        
        _changeRadius = Sequence.Create().Group(Tween.Scale(_collider.transform, _settings).Group(Tween.Scale(_area.transform, _settings)));
        
        _changeRadius.timeScale = _currentTimeScale;

        await _changeRadius.ToYieldInstruction().WithCancellation(_cts.Token);

        Reached?.Invoke();
    }

    private void SetSettings(float targetRadius, float time)
    {
        _targetRadius = targetRadius;
        _time = time;
        
        _settings.startValue = _collider.transform.localScale.x;
        _settings.endValue = _targetRadius;
        _settings.settings.duration = _time;
    }
    
    private void OnThresholdReached() => ThresholdReached?.Invoke();
}
