using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(InputReader))]
public class Player : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private AnimationHandler _controller;
    [SerializeField] private Mover _mover;
    [SerializeField] private PlayerInfo _baseStats;
    
    private CharacterStats _currentStats;
    
    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        SetInitialValues();
    }
    
    private void FixedUpdate()
    {
        _mover.Move(_inputReader.MovementDirection, _currentStats.MovementSpeed);
        _controller.PlayRunAnimation(_inputReader.MovementDirection.magnitude);
    }
    
    private void SetInitialValues()
    {
        if (_baseStats == null)
            throw new Exception();
        
        _currentStats.Health = _baseStats.Health;
        _currentStats.HealthRegeneration = _baseStats.HealthRegeneration;
        _currentStats.Armor = _baseStats.Armor;
        _currentStats.MovementSpeed = _baseStats.MovementSpeed;
        _currentStats.AttackSpeed = _baseStats.AttackSpeed;
        _currentStats.AttackRadius = _baseStats.AttackRadius;
        _currentStats.PickUpRadius = _baseStats.PickUpRadius;
        _currentStats.BlockChance = _baseStats.BlockChance;
        _currentStats.EvasionChance = _baseStats.EvasionChance;
        _currentStats.CritChance = _baseStats.CritChance;
        _currentStats.CritMultiplier = _baseStats.CritMultiplier;
        _currentStats.XpMultiplier = _baseStats.XpMultiplier;
        _currentStats.GoldMultiplier = _baseStats.GoldMultiplier;
        _currentStats.Luck = _baseStats.Luck;
    }
}
