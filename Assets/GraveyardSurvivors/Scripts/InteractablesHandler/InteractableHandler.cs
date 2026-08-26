using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableHandler : MonoBehaviour
{
    [SerializeField] protected InteractableSpawner InteractableSpawner;
    
    protected IPlayer Player;
    
    public void Init(IPlayer player)
    {
        Player = player ?? throw new ArgumentNullException(nameof(player));
    }

    private void OnEnable()
    {
        InteractableSpawner.InteractableWasChosen += OnInteractableChosen;
    }

    private void OnDisable()
    {
        InteractableSpawner.InteractableWasChosen -= OnInteractableChosen;
    }

    public void Spawn(Vector3 position) => InteractableSpawner.Spawn(position);

    protected abstract void OnInteractableChosen<T>(T interactable) where T : Interactable;
}
