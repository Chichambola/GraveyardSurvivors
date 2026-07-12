using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Item : MonoBehaviour, IPoolable<Item>, IPickable, IWeightedObject, IItem
{
    [SerializeField] private ItemInfo _info;
    [SerializeField] private int _weight;

    public event Action<Item> CanBeReleased;
    
    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    public Sprite Sprite => _info.Sprite;
    public string Name => _info.Name;
    public abstract string CurrentDescription { get; }

    public int Weight => _weight;
    
    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    protected virtual void OnValidate()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void ResetCharacteristics() { }

    public void Release() => CanBeReleased?.Invoke(this);
}
