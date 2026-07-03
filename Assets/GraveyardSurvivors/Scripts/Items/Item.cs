using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Item : MonoBehaviour, IPoolable<Item>, IBuff, IPickable, IWeightedObject, IItem
{
    [SerializeField] private ItemInfo _info;
    [SerializeField] private int _weight;
    [SerializeField] protected int IncreaseValue;

    public event Action<Item> CanBeReleased;
    
    private Rigidbody _rigidbody;
    private BoxCollider _collider;

    public Sprite Sprite => _info.Sprite;
    public string Name => _info.Name;
    public abstract string CurrentDescription { get; }

    public int Weight => _weight;
    
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

    public abstract CharacterStats ApplyBuff(CharacterStats baseStats);
    public abstract CharacterStats RemoveBuff(CharacterStats baseStats);
}
