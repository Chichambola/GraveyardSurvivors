using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsShowingValue { get; }
    public bool IsCurrentlyAvailable { get; }
   void SetVisibility(bool value);
    void ProcessInteraction();
}
