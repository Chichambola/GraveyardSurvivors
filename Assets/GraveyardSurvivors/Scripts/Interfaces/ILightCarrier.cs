using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightCarrier
{
    void ResetRadius(float duration);
    void StartChangingRadius();
    void StartLight();
    void PauseLight();
    void Heal(float value);
}
