using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Item : MonoBehaviour, IPoolable<Item>, IPickable, IWeightedObject, IItem
{
    [Header("Item specific fields")]
    [SerializeField] private ItemInfo _info;
    [SerializeField] private int _weight;
    [SerializeField] private bool _isItem = true;

    public event Action<Item> CanBeReleased;
    
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private SpriteRenderer _spriteRenderer;
    private RarityLevel _rarityLevel;

    public Sprite Sprite => _info.Sprite;
    public string Name => _info.Name;
    public abstract string CurrentDescription { get; }
    public ERarityLevel Rarity => _rarityLevel.Rarity; 
    public int Weight => _weight;
    
    protected virtual void Awake()
    {
        if (!_isItem)
            return;
        
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

    public void SetRarityLevel(RarityLevel level)
    {
        _rarityLevel = level;
    }
}
