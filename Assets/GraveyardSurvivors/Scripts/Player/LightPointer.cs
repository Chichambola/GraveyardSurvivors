using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

[RequireComponent(typeof(SphereCollider))]
public class LightPointer : MonoBehaviour
{
    [SerializeField] private MeshRenderer _arrowObject;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float _fadeDuration = 1f;
    
    private IPlayer _player;
    private HashSet<Collider> _collidersToIgnore;
    private ILantern _lantern;
    private SphereCollider _collider;
    private int _fullOpacity = 0;
    private int _fullVisibility = 1;

    public void Init(IPlayer player, ILantern lantern)
    {
        _player = player;
        _lantern = lantern;
        
        var color = _arrowObject.material.color;
        color.a = _fullOpacity;
        _arrowObject.material.color = color;
    }
    
    private void Awake()
    {
        _collidersToIgnore = new HashSet<Collider>();
        _collider = GetComponent<SphereCollider>();
    }

    private void LateUpdate()
    {
        _arrowObject.transform.position = _player.CurrentPosition + _positionOffset;
        
        Vector3 direction = (_lantern.CurrentPosition - _player.CurrentPosition).normalized;
        
        _rotator.Rotate(direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        ProcessCollider(other, _fullOpacity);
    }

    private void OnTriggerExit(Collider other)
    {
        ProcessCollider(other, _fullVisibility);
    }

    private void ProcessCollider(Collider collider, float visibilityValue)
    {
        if (_collidersToIgnore.Contains(collider)) 
            return;
        
        if (collider.TryGetComponent(out IPlayer player) && player == _player)
        {
            Tween.MaterialAlpha(_arrowObject.material, visibilityValue, _fadeDuration);
        }
        else
        {
            _collidersToIgnore.Add(collider);
        }
    }
}
