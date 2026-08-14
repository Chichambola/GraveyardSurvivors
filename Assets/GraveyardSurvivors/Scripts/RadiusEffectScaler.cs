
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using Sequence = PrimeTween.Sequence;
using Tween = PrimeTween.Tween;

public class RadiusEffectScaler : MonoBehaviour, IValueOwner
{
    [SerializeField] private float _radius = 3f;
    [SerializeField] private ParticleSystem _area;
    [SerializeField] private SphereCollider _collider;
    [SerializeField] private Ease _easeType = Ease.InCubic;
    [SerializeField] private bool _show = false;
    
    private Sequence _changeRadius;
    private CancellationTokenSource _cts;
    private TweenSettings<float> _settings;
    private float _initialRadius;
    private float _currentTimeScale;
    private float _targetRadius;
    private float _time;

    public float Value => _collider.transform.localScale.x;
    public float InitialValue => _initialRadius;
    public float TimeScale => _currentTimeScale;
    
    private void Awake()
    {
        _initialRadius = _radius;
        _settings = new TweenSettings<float>();
        
        _settings.settings.ease = _easeType;
        
        _collider.radius = 1;
        _currentTimeScale = 1;
    }

    private void Update()
    {
        if (_show)
        {
            Debug.Log(_currentTimeScale);
        }
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

    public void SetTimeScale(float timeScale)
    {
        _currentTimeScale = timeScale;
    }
    
    public void StopChanging()
    {
        if (!_changeRadius.isAlive)
            return;
        
        _changeRadius.Stop();
        _cts?.Cancel();
    }

    public void ChangeRadius(float targetRadius, float time)
    {
        StopChanging();
        
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
        
        StopChanging();
    }

    private void SetSettings(float targetRadius, float time)
    {
        _targetRadius = targetRadius;
        _time = time;
        
        _settings.startValue = _collider.transform.localScale.x;
        _settings.endValue = _targetRadius;
        _settings.settings.duration = _time;
    }
}
