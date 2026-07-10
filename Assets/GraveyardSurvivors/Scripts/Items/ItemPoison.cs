using System.Collections;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
using UnityEngine;

public class ItemPoison : Item, IAttackItem
{
    [SerializeField] private DamageOverTimeFactory _poisonEffect;
    
    private Effect _effect;

    public override string CurrentDescription => $"Adding {_poisonEffect.Chance}% chance to poison enemies on hit";

    public override void Apply(IAttacker attacker)
    {
        if (attacker is not Player _)
            return;

        _effect = new Effect();

        Debug.Log(_effect.Count);
        
        _effect.SetEffects(_poisonEffect);
        
        PlayerHandler.AddEffect(_effect);
    }
}
 