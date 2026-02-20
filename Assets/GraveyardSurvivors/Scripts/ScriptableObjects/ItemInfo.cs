using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Items/New Item")]
public class ItemInfo : ScriptableObject, IWeightedObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _weight;
    [SerializeField] private ERarityLevel _rarityLevel;
    
    public string Name => _name;
    public string Description => _description;
    public ERarityLevel Rarity => _rarityLevel;
    public Sprite Sprite => _sprite;
    public int Weight => _weight;
}
