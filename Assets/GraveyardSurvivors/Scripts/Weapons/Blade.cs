using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Blade : MonoBehaviour, IPoolable<Blade>
{
    [SerializeField] private float _yOffset = 25f;
    
    public event Action<Blade> CanBeReleased;

    private Quaternion _initialRotation;
    private Tween _tween;
    private TweenSettings<Quaternion> _tweenSettings;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnValidate()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnEnable()
    {
        _initialRotation = transform.rotation;
    }
    
    public void ResetCharacteristics()
    {
        transform.rotation = _initialRotation;
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
        var endValue = new Quaternion(_initialRotation.x, _initialRotation.y + _yOffset, _initialRotation.z, _initialRotation.w);
        
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
        _tween = Tween.Rotation(transform, _tweenSettings).OnComplete(Release);

        await _tween;
    }
}