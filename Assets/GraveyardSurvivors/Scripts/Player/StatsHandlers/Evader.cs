using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : MonoBehaviour
{
    public bool CanEvade(float currentEvasionChance, float luck)
    {
        currentEvasionChance += luck;
        
        int randomPercent = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);

        return !(randomPercent > currentEvasionChance);
    }
}
