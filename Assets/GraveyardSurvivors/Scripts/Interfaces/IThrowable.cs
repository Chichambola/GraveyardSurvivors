using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    void StartMoving();
    void SetPosition(Vector3 position);
}
