using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserUtils
{
    public static readonly int HighestPercent = 100;
    public static readonly int LowestPercent = 100;

    public static float AddPercentToNumber(float originalNumber, float percent)
    {
        float finalNumber = originalNumber * (1 + percent / HighestPercent);

        return finalNumber;
    }

    public static float SubstractPercentFromNumber(float originalNumber, float percent)
    {
        float finalNumber = originalNumber * (1 - percent / HighestPercent);

        return finalNumber;
    }
}
