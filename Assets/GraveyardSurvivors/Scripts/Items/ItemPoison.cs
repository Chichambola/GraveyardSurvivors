using System.Collections;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
using UnityEngine;

public class ItemPoison : Item, IAttackItem
{
    [SerializeField] private DamageOverTimeFactory _poisonEffect;
    
    private Effect _effect;
    private IPlayer _player;

    public override string CurrentDescription => $"Adding {_poisonEffect.Chance}% chance to poison enemies on hit";
    
    public void SetPlayer(IPlayer player)
    {
        if (_player != null)
            throw new System.Exception("Player is already set");
        
        _player = player;
    }
    
    public void Apply()
    {
        _effect = new Effect();
        
        _effect.SetEffects(_poisonEffect);

        _player?.AddEffect(_effect);
    }
}
 