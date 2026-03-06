using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class Item : MonoBehaviour, IPoolable<Item>, IBuff, IPickable
{
    [SerializeField] private ItemInfo _info;
    [SerializeField] protected int IncreaseValue;

    public event Action<Item> CanBeReleased;

    protected float HighestValue = UserUtils.s_HighestPercent;
    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private IBuff _buffImplementation;

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

    public abstract CharacterStats ApplyBuff(CharacterStats baseStats);
    public abstract CharacterStats RemoveBuff(CharacterStats baseStats);

    protected float CalculateBuffAmount(float value)
    {
        if (value >= HighestValue)
            return value;

        float currentPercent = UserUtils.SubtractPercentFromNumber(HighestValue, value);

        float finalAvailablePercent = UserUtils.SubtractPercentFromNumber(currentPercent, IncreaseValue);

        value += currentPercent - finalAvailablePercent;

        return value;
    }
}
