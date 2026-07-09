using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoison : Item, IAttackItem
{
    [SerializeField] private Effect _effect;
    
    public override string CurrentDescription { get; }

    public override void Apply(IAttacker attacker)
    {
        if (attacker is not Player _)
            return;
            
        PlayerHandler.AddEffect(_effect);
    }
}
 