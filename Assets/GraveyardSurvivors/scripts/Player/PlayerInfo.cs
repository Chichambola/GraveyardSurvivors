using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Characters/New Player")]
public class PlayerInfo : ScriptableObject
{
    [SerializeField] private float _health;
    [SerializeField] private float _healthRegeneration;
    [SerializeField] private float _armorPercent;
    [SerializeField] private float _movementSpeedPercent;
    [SerializeField] private float _attackSpeedPercent;
    [SerializeField] private float _attackRadiusPercent;
    [SerializeField] private float _pickUpRadiusPercent;
    [SerializeField] private float _blockChancePercent;
    [SerializeField] private float _critChancePercent;
    [SerializeField] private float _critMultiplier;
    [SerializeField] private float _xpMultiplier;
    [SerializeField] private float _goldMultiplier;
    [SerializeField] private float _evasionChancePercent;
    [SerializeField] private float _luckPercent;

    public float Health => _health;
    public float HealthRegeneration => _healthRegeneration;
    public float Armor => _armorPercent;
    public float MovementSpeed => _movementSpeedPercent;
    public float AttackSpeed => _attackSpeedPercent;
    public float AttackRadius => _attackRadiusPercent;
    public float PickUpRadius => _pickUpRadiusPercent;
    public float BlockChance => _blockChancePercent;
    public float EvasionChance => _evasionChancePercent;
    public float CritChance => _critChancePercent;
    public float CritMultiplier => _critMultiplier;
    public float XpMultiplier => _xpMultiplier;
    public float GoldMultiplier => _goldMultiplier;
    public float Luck => _luckPercent;
}
