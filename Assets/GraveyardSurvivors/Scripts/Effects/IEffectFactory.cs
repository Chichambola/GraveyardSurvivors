using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectFactory<IAttacker>
{
    IEffect<IAttacker> Create();
}
