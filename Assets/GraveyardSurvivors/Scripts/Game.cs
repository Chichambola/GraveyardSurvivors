using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private List<InteractableHandler> _interactables;

    private void OnEnable()
    {
        if(_interactables == null) 
            throw new Exception("Interactables are null");

        foreach (var interactables in _interactables)
        {
            if (interactables.TryGetComponent(out IInteractableHandler handler))
            {
                handler.Init(_player);
            }
        }
    }
}
