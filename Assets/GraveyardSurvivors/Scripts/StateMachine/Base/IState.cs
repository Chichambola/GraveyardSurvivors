using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void DoExit();
    void DoEnter();
    void FixedUpdate();
    void Update();
}
