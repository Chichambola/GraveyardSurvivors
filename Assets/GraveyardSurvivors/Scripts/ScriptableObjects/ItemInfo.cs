using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Items/New Item")]
public class ItemInfo : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private RarityLevel _rarityLevel;
    
    public string Name => _name;
    public string Description => _description;
    public RarityLevel Rarity => _rarityLevel;
    private Sprite Sprite => _sprite;
}
