using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private InteractablesDetector _detector;

    private void OnEnable()
    {
        _player.InteractionButtonPressed += OnInteractionButtonPressed;
    }

    private void OnDisable()
    {
        _player.InteractionButtonPressed -= OnInteractionButtonPressed;
    }

    private void OnInteractionButtonPressed()
    {
        if (_detector.TryGetInteractable(out IInteractable interactable))
        {
            interactable.ProcessInteraction();
        }
    }
}
