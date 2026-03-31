using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : CostInteractableHandler
{
    [SerializeField] protected ItemsHandler ItemsHandler;
    [SerializeField] private RarityEvaluator _rarityEvaluator;
    
    protected ERarityLevel GetRarityLevel(IChanceInteractable interactable)
    {
        int commonChance = interactable.CommonChance;
        int rareChance = interactable.RareChance;
        int legendaryChance = interactable.LegendaryChance;
        
        ERarityLevel rarityLevel = _rarityEvaluator.GetRarityLevel(commonChance, rareChance, legendaryChance);
        return rarityLevel;
    }
    
    protected ERarityLevel GetRarityLevel(IChanceInteractable interactable, int noneChance)
    {
        int commonChance = interactable.CommonChance;
        int rareChance = interactable.RareChance;
        int legendaryChance = interactable.LegendaryChance;
        
        ERarityLevel rarityLevel = _rarityEvaluator.GetRarityLevel(noneChance, commonChance, rareChance, legendaryChance);
        return rarityLevel;
    }
}
