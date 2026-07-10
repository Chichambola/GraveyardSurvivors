using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectFactory<IAttacker>
{
    void SetParticleEffectSpawner(ParticleEffectSpawner spawner);
    IEffect<IAttacker> Create();
}
