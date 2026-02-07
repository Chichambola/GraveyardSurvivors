using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] private ItemInfo _info;
    [SerializeField] protected int InscreaseValue;
    
    public ItemInfo Info => _info;
}
