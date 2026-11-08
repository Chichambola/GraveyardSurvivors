using System;
using UnityEngine;

public class InteractableHandlerBase : MonoBehaviour
{
    [SerializeField] private InteractableSpawner _interactableSpawner;
    [SerializeField] private int _initialSpawnCount = 10;
    
    public event Action<Interactable> HandleChosenObject;
    
    private IPlayer _player;

    public int InitialCount => _initialSpawnCount;

    private void OnEnable()
    {
        _interactableSpawner.InteractableWasChosen += OnInteractableChosen;
    }
    
    private void OnDisable()
    {
        _interactableSpawner.InteractableWasChosen -= OnInteractableChosen;
    }
    
    private void OnValidate()
    {
        if (_initialSpawnCount <= 0)
        {
            _initialSpawnCount = 0;
        }
    }

    public Interactable Spawn(Vector3 position)
    {
        var interactable = _interactableSpawner.Spawn(position);

        return interactable;
    }
    
    public void SetValueForObjects(float value)
    {
        _interactableSpawner.SetValueForObjects(value);
    }
    
    private void OnInteractableChosen(Interactable interactable)
    {
        HandleChosenObject?.Invoke(interactable);
    }
}