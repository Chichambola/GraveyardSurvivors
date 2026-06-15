using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : MonoBehaviour
{
    public bool CanEvade(float currentEvasionChance, float luck)
    {
        if (currentEvasionChance <= 0)
            return false;
        
        currentEvasionChance += luck;
        
        float randomPercent = Random.Range(UserUtils.s_LowestPercent, UserUtils.s_HighestPercent);
        
        return !(randomPercent > currentEvasionChance);
    }
}
