using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable, IPoolable
{
    public const string IsOpened = nameof(IsOpened); 
    
    [SerializeField] private float _cost;
    [SerializeField] private Outline _outline;
    [SerializeField] private Animator _animator;
    [SerializeField] private QuadraticCurvePoints _points;
    
    public event Action<Chest> CanBeReleased;
    
    public QuadraticCurvePoints Points => _points;
    
    public float Cost { get; private set; }

    private void OnEnable()
    {
        if (_cost <= 0)
            throw new Exception(nameof(_cost));

        Cost = _cost;
    }

    public void ChangeOutlineVisibility(bool value)
    {
        _outline.enabled = value;
    }

    public void ProcessInteraction()
    {
        _animator.SetBool(IsOpened, true);
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
