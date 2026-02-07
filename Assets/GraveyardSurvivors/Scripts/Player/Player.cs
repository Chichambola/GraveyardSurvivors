using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour, IBuffable
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private AnimationHandler _controller;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private PlayerInfo _baseStats;

    private readonly List<IBuff> _buffs = new ();
    
    public CharacterStats CurrentStats { get; private set; }
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        if (_baseStats == null)
            throw new Exception();

        CurrentStats = _baseStats.Stats;
    }

    private void FixedUpdate()
    {
        _mover.Move(_inputReader.MovementDirection.normalized, CurrentStats.MovementSpeed);
        _rotator.Rotate(_inputReader.MovementDirection.normalized);
        _controller.PlayMovementAnimation(_inputReader.MovementDirection.magnitude);
    }

    public void AddBuff(IBuff buff)
    {
        _buffs.Add(buff);
        
        ApplyBuffs();
    }

    public void RemoveBuff(IBuff buff)
    {
        _buffs.Remove(buff);
        
        ApplyBuffs();
    }

    private void ApplyBuffs()
    {
        CurrentStats = _baseStats.Stats;

        foreach (var buff in _buffs)
        {
            CurrentStats = buff.ApplyBuff(CurrentStats);
        }
    }
}
