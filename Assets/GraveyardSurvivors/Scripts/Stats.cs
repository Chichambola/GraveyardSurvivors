using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats<T> : MonoBehaviour where T : IStat
{
    public abstract void UpdateStats(T stats);
}
