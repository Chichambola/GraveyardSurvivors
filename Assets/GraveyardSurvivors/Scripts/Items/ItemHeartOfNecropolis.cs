using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHeartOfNecropolis : Item
{
    public override CharacterStats ApplyBuff(CharacterStats baseStats)
    {
        throw new InvalidImplementationException();
    }
}
