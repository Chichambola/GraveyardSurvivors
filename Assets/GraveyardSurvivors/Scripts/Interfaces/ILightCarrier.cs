using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILightCarrier
{
    public LanternLight Light { get; }
    public int LanternsCount { get; }
    void IncreaseLanternCount();
    void DecreaseLanternCount();
}
