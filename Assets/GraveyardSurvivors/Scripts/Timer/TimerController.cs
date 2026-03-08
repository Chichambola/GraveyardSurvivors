using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimerController
{
    private static List<Timer> s_Timers = new();

    public static void RegisterTimer(Timer timer) => s_Timers.Add(timer);
    public static void DeregisterTimer(Timer timer) => s_Timers.Remove(timer);

    public static void UpdateTimers()
    {
        if (s_Timers.Count > 0)
        {
            foreach (var timer in new List<Timer>(s_Timers))
            {
                timer?.Tick();
            }   
        }
    }

    public static void Clear() => s_Timers.Clear();
}