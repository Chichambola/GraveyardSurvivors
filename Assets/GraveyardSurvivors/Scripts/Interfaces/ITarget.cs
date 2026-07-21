using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    public event Action WasReached;
    void SetFollower(IFollower follower);
    void SetPosition(Vector3 position);
}
