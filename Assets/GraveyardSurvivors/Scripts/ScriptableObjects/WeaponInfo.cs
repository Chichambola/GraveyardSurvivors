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
    [SerializeField] private float _damage;
    [SerializeField] private float _percentMultiplierPerWeapon;
}
