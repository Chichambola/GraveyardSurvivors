using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableSpawner : Spawner<Interactable>
{
    private Vector3 _spawnPoint;
    
    public event Action<Interactable> InteractableWasChosen;

    public void Spawn(Vector3 position)
    {
        _spawnPoint = position;

        GetObject();
    }

    public void SetValueForObjects(float value)
    {
        if (ActiveObjects.Count <= 0)
            throw new Exception("List's count has to be greater than 0");
        
        foreach (var interactable in ActiveObjects)
        {
            interactable.SetValue(value);
        }
    }
    
    protected override void ActionOnGet(Interactable interactable)
    {
        ActiveObjects.Add(interactable);
        
        interactable.transform.position = _spawnPoint;
        interactable.transform.parent = transform;

        interactable.WasChosen += OnInteractableChosen;
        interactable.CanBeReleased += Release;
        
        base.ActionOnGet(interactable);
    }

    protected override void ActionOnRelease(Interactable interactable)
    {
        ActiveObjects.Remove(interactable);
        
        interactable.WasChosen -= OnInteractableChosen;
        interactable.CanBeReleased -= Release;
        
        base.ActionOnRelease(interactable);
    }

    private void OnInteractableChosen(Interactable interactable)
    {
        InteractableWasChosen?.Invoke(interactable);
    }
}
