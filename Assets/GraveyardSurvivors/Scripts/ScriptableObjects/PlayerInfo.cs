using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Characters/New character")]
public class PlayerInfo : ScriptableObject
{
    [SerializeField] private CharacterStats _characterStats;

    public CharacterStats GetStats()
    {
        return new CharacterStats(_characterStats);
    }
}
