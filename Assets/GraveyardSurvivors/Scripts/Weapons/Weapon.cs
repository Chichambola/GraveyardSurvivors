using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Weapon : MonoBehaviour, IWeapon, IItem, IWeightedObject
{
    [SerializeField] private ItemInfo _itemInfo;
    [Header("Weapon specific fields")]
    [SerializeField] protected float BonusDamagePerUpgrade = 1;
    [SerializeField] protected float Cooldown = 1f;
    [SerializeField] private float _damage;
    
    private float _bonusDamage;
    private string _description;
    
    public virtual event Action<IAttacker, IWeapon> AttackerDetected;
    
    public abstract string BaseDescription { get; }
    public abstract string UpgradeDescription { get; }
    public bool IsAttacking { get; protected set; }
    public float Damage => _damage + _bonusDamage;
    public float CurrentCooldown => Cooldown;
    public int Weight => _itemInfo.Weight;
    public string CurrentDescription => _description;
    public Sprite Sprite => _itemInfo.Sprite;
    public string Name => _itemInfo.Name;
    public ERarityLevel Rarity => _itemInfo.Rarity;

    private void OnDisable()
    {
        _bonusDamage = 0;
    }

    public virtual void Upgrade()
    {
        _bonusDamage += BonusDamagePerUpgrade;
    }

    public virtual void Reset() { }

    public void SetDescription(string description) => _description = description;

    public virtual void SetCooldown(float cooldown) => Cooldown = cooldown;

    public abstract void Attack();

    public abstract void StopAttacking();
}
