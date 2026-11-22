using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Blade : MonoBehaviour, IPoolable<Blade>
{
    [SerializeField] private float _yOffset = 25f;
    
    public event Action<Blade> CanBeReleased;

    private Vector3 _initialRotation;
    private Tween _tween;
    private TweenSettings<Vector3> _tweenSettings;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnEnable()
    {
        _initialRotation = transform.rotation.eulerAngles;
    }

    public void ResetCharacteristics()
    {
        var rotation = transform.rotation;
        rotation.eulerAngles = _initialRotation;
        transform.rotation = rotation;
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }

    public void SetVisibility(float duration)
    {
        _tweenSettings.settings.duration = duration;
    }
    
    public void StartSpin()
    {
        _tweenSettings.startValue = _initialRotation;
        var endValue = new Vector3(_initialRotation.x, _initialRotation.y + _yOffset, _initialRotation.z);
        
        _tweenSettings.endValue = endValue;

        Spin().Forget();
    }

    public void IncreaseSize(float gain)
    {
        var xScale = transform.localScale.x.AddPercentToNumber(gain);
        var yScale = transform.localScale.y.AddPercentToNumber(gain);
        var zScale = transform.localScale.z.AddPercentToNumber(gain);

        transform.localScale = new Vector3(xScale, yScale, zScale);
    }
    
    private async UniTask Spin()
    {
        _tween = Tween.LocalRotation(transform, _tweenSettings).OnComplete(Release);

        await _tween;
    }
}