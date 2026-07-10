using System.Collections;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
using UnityEngine;

public class ItemPoison : Item, IAttackItem
{
    [SerializeField] private Effect _effect;
    [SerializeField] private DamageOverTimeFactory _poisonEffect;

    public override string CurrentDescription => $"Adding {_poisonEffect.Chance}% chance to poison enemies on hit";

    public override void Apply(IAttacker attacker)
    {
        if (attacker is not Player _)
            return;
        
        var spawner = Instantiate(_poisonEffect.Spawner, Vector3.one, Quaternion.identity);
        
        _poisonEffect.SetParticleEffectSpawner(spawner);
        
        _effect.SetEffects(_poisonEffect);
        
        PlayerHandler.AddEffect(_effect);
    }
}
 