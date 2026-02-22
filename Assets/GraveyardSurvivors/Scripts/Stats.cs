using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats<T> : MonoBehaviour where T : IStat
{
    protected abstract void OnEnable();
    protected abstract void OnDisable();
    protected abstract void OnStatsChanged(T stats);
    public abstract void SetInitialStats(T stats);
}
