using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using Unity.Mathematics;
using UnityEditor.Profiling;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractablesHandler : MonoBehaviour, IHandler
{
    [SerializeField] private SerializableDictionary<InteractableHandler, int> _interactables;
    [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;
    [SerializeField] private int _errorThreshold = 10;
    
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

                int triesAmount = 0;
                
                while (!isPlaced)
                {
                    Vector3 position = _spawnCollidersHandler.GetRandomPosition();

                    if (!_placementVerifier.IsPlacementValid(position) && triesAmount <= _errorThreshold)
                    {
                        triesAmount++;
                        
                        continue;
                    }
                    
                    isPlaced = true;
                    
                    interactable.Init(player);

                    interactable.Spawn(position);
                }
                
                if (interactable is IPriceOwner priceOwner)
                {
                    priceOwner.InitializePrices();
                } 
            }
        }
        
        Physics.autoSyncTransforms = false;
    }
}