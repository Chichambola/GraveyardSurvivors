using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public Sprite Sprite { get; }
    public string Name { get; }
    public string CurrentDescription { get; }
    public ERarityLevel Rarity { get; }
}
