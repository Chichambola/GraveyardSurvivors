using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHeartOfNecropolis : Item, IBuff
{
    [SerializeField] private int _increaseValue = 30;
    [SerializeField] private int _healthRegenDecreasePercent = 50;
    
    public override string CurrentDescription => $"+{_increaseValue} Max HP at the cost of -{_healthRegenDecreasePercent}%HP Regen. ";

    protected void OnValidate()
    {
        if (_healthRegenDecreasePercent < 0)
        {
            _healthRegenDecreasePercent = 0;
        }

        if (_healthRegenDecreasePercent > 100)
        {
            _healthRegenDecreasePercent = 100;
        }
    }

    public CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth += _increaseValue;
        
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.SubtractPercentFromNumber(_healthRegenDecreasePercent);
        
        return baseStats;
    }
    
    public CharacterStats RemoveBuff(CharacterStats baseStats)
    {
        baseStats.MaxHealth -= _increaseValue;
        
        baseStats.HealthRegeneration = baseStats.HealthRegeneration.AddPercentToNumber(_healthRegenDecreasePercent);
        
        return baseStats;
    }
}
