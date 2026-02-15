using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable<T>
{
    public event Action<T> CanBeReleased;
    void ResetCharacteristics();
    void Release();
}
