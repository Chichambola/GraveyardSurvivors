using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerController
{
    private static List<Timer> _timers = new();

    public static void RegisterTimer(Timer timer) => _timers.Add(timer);
    public static void DeregisterTimer(Timer timer) => _timers.Remove(timer);

    public static void UpdateTimer()
    {
        foreach (var timer in _timers)
        {   
            timer.Tick();
        }
    }

    public static void Clear() => _timers.Clear();
}