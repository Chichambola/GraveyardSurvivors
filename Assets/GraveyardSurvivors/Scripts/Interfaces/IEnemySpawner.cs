using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public interface IEnemySpawner<T> 
{
    public event Action<T> EnemyWasReleased;
    public event Action<T> EnemyWasSpawned;
    public int Weight { get; }
    public int Cost { get; }
}
