using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    public Transform Transform { get; }
    public QuadraticCurvePoints Points { get; }
    void StartMoving();
}
