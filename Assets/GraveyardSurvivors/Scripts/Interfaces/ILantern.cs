using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILantern
{
    void StopShrinking();
    void StartShrinking(float rate);
    void StartExpanding(float radius);
    void StopLight();
}
