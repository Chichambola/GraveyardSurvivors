using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : MonoBehaviour
{
    private float _evasionPercent;
    
    public void SetInitialStats(float evasionPercent)
    {
        _evasionPercent = evasionPercent;
    }

    public bool CanEvade(float luck)
    {
        float currentEvasionChance = _evasionPercent + luck;
        
        int randomPercent = Random.Range(UserUtils.LowestPercent, UserUtils.HighestPercent);

        return !(randomPercent > currentEvasionChance);
    }
}
