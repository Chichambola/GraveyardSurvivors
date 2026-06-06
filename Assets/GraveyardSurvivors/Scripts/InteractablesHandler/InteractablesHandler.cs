using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using Unity.Mathematics;
using UnityEditor.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractablesHandler : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<InteractableHandler, int> _interactables;
    [SerializeField] private List<Collider> _colliders;
    [SerializeField] private PlacementVerifier _placementVerifier;
    
    private void Awake()
    {
        if (_colliders.Count <= 0)
        {
            throw new Exception($"Colliders length can not be less than 0");
        }
    }

    public void Init(IPlayer player)
    {
        if (_interactables == null)
            throw new Exception("Interactables are null");

        Physics.autoSyncTransforms = true;
        
        foreach (var interactable in _interactables.Keys)
        {
            int count = _interactables[interactable];

            for (int i = 0; i < count; i++)
            {
                bool isPlaced = false;
                
                while (!isPlaced)
                {
                    Collider spawnCollider = GetCollider();

                    Vector3 position = GetPosition(spawnCollider);
                    
                    if (!_placementVerifier.IsPlacementValid(position)) continue;
                    
                    isPlaced = true;
                    
                    interactable.Init(player);

                    interactable.Spawn(position);
                }
                
                if (interactable is CostInteractableHandler costInteractable)
                {
                    costInteractable.SetValueForObjects();
                } 
            }
        }
        
        Physics.autoSyncTransforms = false;
    }

    private Vector3 GetPosition(Collider collider)
    {
        float spawnAreaMinX = collider.bounds.min.x;
        float spawnAreaMaxX = collider.bounds.max.x;

        float spawnAreaMinZ = collider.bounds.min.z;
        float spawnAreaMaxZ = collider.bounds.max.z;

        float positionX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float positionY = collider.bounds.max.y;
        float positionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        return new Vector3(positionX, positionY, positionZ);
    }

    private Collider GetCollider()
    {
        int randomIndex = Random.Range(0, _colliders.Count);

        return _colliders[randomIndex];
    }
}