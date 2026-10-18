using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemHeartOfNecropolis : Item, IBuff
{
    [Header("Max health")]
    [SerializeField] private int _increaseValue = 30;
    [Header("Health regeneration")]
    [SerializeField] private int _decreasePercent = 50;
    
    public override string CurrentDescription => $"+{_increaseValue} Max HP at the cost of -{_decreasePercent}%HP Regen. ";

    protected void OnValidate()
    {
        if (_decreasePercent < 0)
        {
            _decreasePercent = 0;
        }

        if (_decreasePercent > 100)
        {
            _decreasePercent = 100;
        }
    }

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth += _increaseValue;
        
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.SubtractPercentFromNumber(_decreasePercent);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= _increaseValue;
        
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.AddPercentToNumber(_decreasePercent);
        
        return baseStats;
    }
}
