using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Characters/New Player")]
public class PlayerInfo : ScriptableObject
{
    [SerializeField] private CharacterStats _characterStats;

    public CharacterStats Stats => _characterStats;
}
