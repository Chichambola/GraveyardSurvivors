using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool IsCurrentlyAvailable { get; }
    void ChangeOutlineVisibility(bool value);
    void ProcessInteraction();
}
