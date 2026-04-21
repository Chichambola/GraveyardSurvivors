using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PrimeTween;
using TMPro;
using TreeEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Ease = PrimeTween.Ease;
using Tween = PrimeTween.Tween;

public class Chest : Interactable, IChanceInteractable
{
    [Header("Lid opening")]
    [SerializeField] private Transform _openingPart;
    [SerializeField] private float _openingDuration = 2f;
    [SerializeField] private Quaternion _openingRotation;
    [SerializeField] private Ease _openingEase;
    [SerializeField] private float _countdownTime = 3;
    [Header("Weights")]
    [SerializeField] protected int CommonChanceWeight;
    [SerializeField] protected int RareChanceWeight;
    [SerializeField] protected int LegendaryChanceWeight;
    
    public override event Action<Interactable> CanBeReleased;

    private Coroutine _coroutine;
    private Tween _openingPartTween;
    private TweenSettings<Vector3> _openingSettings;
    private Quaternion _defaultRotation;

    public int CommonChance => CommonChanceWeight;
    public int RareChance => RareChanceWeight;
    public int LegendaryChance => LegendaryChanceWeight;

    private void Awake()
    {
        _openingSettings = new TweenSettings<Vector3>(_openingRotation.eulerAngles, _openingDuration, _openingEase);
        _defaultRotation = _openingPart.transform.localRotation;
    }

    private void OnValidate()
    {
        if (_openingDuration > _countdownTime)
        {
            _openingDuration = _countdownTime;
        }
    }

    private void OnDisable()
    {
        _openingPart.localRotation = _defaultRotation;
        
        _openingPartTween.Stop();
    }

    public void Open()
    {
        _openingPartTween = Tween.LocalRotation(_openingPart, _openingSettings).OnComplete(Release);
        
        IsAvailable = false;
        
        SetVisibility(false);
    }

    public override void Release()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ReleaseCountdown());
    }

    public override void ResetCharacteristics()
    {
        IsAvailable = true;
    }
    
    private IEnumerator ReleaseCountdown()
    {
        float timePassed = 0;
        
        while (timePassed < _countdownTime)
        {
            timePassed += Time.deltaTime;
            
            yield return null;
        }
        
        ResetCharacteristics();
        
        CanBeReleased?.Invoke(this);
    }
}
