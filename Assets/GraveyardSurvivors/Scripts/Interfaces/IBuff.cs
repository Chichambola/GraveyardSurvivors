using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    CharacterStats ApplyBuff(CharacterStats baseStats);
    CharacterStats RemoveBuff(CharacterStats baseStats);
}
