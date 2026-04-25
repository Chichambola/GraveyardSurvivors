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
    [SerializeField] private RarityLevel _common;
    [SerializeField] private RarityLevel _rare;
    [SerializeField] private RarityLevel _legendary;
    
    public override event Action<Interactable> CanBeReleased;

    private Coroutine _coroutine;
    private Tween _openingPartTween;
    private TweenSettings<Vector3> _openingSettings;
    private Quaternion _defaultRotation;
    private List<RarityLevel> _weights;
    
    public List<RarityLevel> Weights => _weights;

    private void Awake()
    {
        _weights = new() { _common, _rare, _legendary};
        
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
        
        foreach (var rarityLevel in _weights)
        {
            rarityLevel.ResetChance();
        }
    }
    
    public void IncreaseChance(float multiplier)
    {
        _legendary.IncreaseWeight(multiplier);
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
