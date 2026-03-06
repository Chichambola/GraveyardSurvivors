using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class UserUtils
{
    public static readonly int s_HighestPercent = 100;
    public static readonly int s_LowestPercent = 0;
    public static readonly int s_MinRotation = 0;
    public static readonly int s_HighestRotation = 360;

    public static float AddPercentToNumber(float originalNumber, float percent)
    {
        float finalNumber = originalNumber * (1 + percent / s_HighestPercent);

        return finalNumber;
    }

    public static float SubtractPercentFromNumber(float originalNumber, float percent)
    {
        float finalNumber = originalNumber * (1 - percent / s_HighestPercent);

        return finalNumber;
    }
    
    public static Vector3 GetDirection(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 aimDirection = (startPosition - endPosition).normalized;

        return aimDirection;
    }
    
    public static float GetRandomRotation()
    {
        return Random.Range(s_MinRotation, s_HighestRotation);
    }
}
