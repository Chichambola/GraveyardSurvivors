using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Weapons/New Weapon")]
public class WeaponInfo : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private float _damage;
    
    public float Damage => _damage;
    public Sprite Sprite => _sprite;
    public string Name => _name;
    public string Description => _description;
}
