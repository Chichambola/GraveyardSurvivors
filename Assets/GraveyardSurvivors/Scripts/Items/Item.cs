using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Item : MonoBehaviour, IPoolable<Item>, IPickable, IWeightedObject, IItem
{
    [SerializeField] private ItemInfo _info;
    
    public event Action<Item> CanBeReleased;
    
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private SpriteRenderer _spriteRenderer;

    public Sprite Sprite => _info.Sprite;
    public string Name => _info.Name;
    public abstract string CurrentDescription { get; }
    public ERarityLevel Rarity => _info.Rarity; 
    public int Weight => _info.Weight;
    
    protected virtual void Awake()
    { 
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetCharacteristics() { }

    public void Release() => CanBeReleased?.Invoke(this);

    public void Hide()
    {
        _collider.enabled = false;
        _spriteRenderer.enabled = false;
    }
}
