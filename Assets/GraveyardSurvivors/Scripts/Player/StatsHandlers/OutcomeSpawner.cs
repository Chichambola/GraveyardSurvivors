using System;
using PrimeTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class OutcomeSpawner : Spawner<Outcome>
{
    [Header("Tween settings")]
    [SerializeField] private float _duration = 2f;
    [SerializeField] private float _yOffset;
    [SerializeField] private Transform _spawnPoint;

    private const float MinXOffset = 0;
    private const float MaxXOffset = 2;
    private TweenSettings<float> _tweenSettings;

    protected override void Awake()
    {
        base.Awake();

        _tweenSettings = new TweenSettings<float>();
    }

    private void OnEnable()
    {
        _tweenSettings.settings.duration = _duration;
    }

    public void Spawn(Sprite sprite)
    {
        var outcome = GetObject();
        
        outcome.Set(sprite);
    }

    protected override void ActionOnGet(Outcome outcome)
    {
        base.ActionOnGet(outcome);

        var randomOffset = Random.Range(MinXOffset, MaxXOffset);
        
        var pos = new Vector3(_spawnPoint.position.x + randomOffset, _spawnPoint.position.y, _spawnPoint.position.z);

        outcome.transform.position = pos;

        _tweenSettings.startValue = outcome.transform.position.y;
        _tweenSettings.endValue = outcome.transform.position.y + _yOffset;
        
        outcome.transform.parent = null;

        outcome.CanBeReleased += Release;

        Tween.LocalPositionY(outcome.transform, _tweenSettings)
            .OnComplete(outcome.Release);
    }

    protected override void ActionOnRelease(Outcome outcome)
    {
        base.ActionOnRelease(outcome);

        outcome.transform.parent = transform;
        
        outcome.CanBeReleased -= Release;
    }
}