using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public Sprite Sprite { get; }
    public string Name { get; }
    public string Description { get; }
    public ERarityLevel Rarity { get; }
}
