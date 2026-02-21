using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternHealBuff : MonoBehaviour, IBuff
{
    [SerializeField] private float _increaseValue = 1.5f;
    
    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration += _increaseValue;

        return baseStats;
    }

    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.HealthRegeneration -= _increaseValue;

        return baseStats;
    }
}
