using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect<IAttacker>
{
    void Apply(IAttacker attacker);
    void Cancel();
}
