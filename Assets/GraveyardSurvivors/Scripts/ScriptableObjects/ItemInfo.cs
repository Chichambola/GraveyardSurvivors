using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Items/New Item")]
public class ItemInfo : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private RarityLevel _rarityLevel;
    
    public Sprite Sprite => _sprite;
    public ERarityLevel Rarity => _rarityLevel.Rarity;
    public string Name => _name;
    public int Weight => _rarityLevel.Weight;
}