using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Item : MonoBehaviour, IPoolable<Item>
{
    [SerializeField] private ItemInfo _info;
    [SerializeField] protected int InscreaseValue;

    public event Action<Item> CanBeReleased;

    protected float HighestValue = UserUtils.HighestPercent;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    
    public Rigidbody Rigidbody => _rigidbody;
    public ItemInfo Info => _info;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnValidate()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void ResetCharacteristics() { }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
}
