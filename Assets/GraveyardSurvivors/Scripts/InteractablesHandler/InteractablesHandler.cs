using System;
using System.Collections;
using System.Collections.Generic;
using Sherbert.Framework.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractablesHandler : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<InteractableHandler, int> _interactables;
    [SerializeField] private Collider[] _colliders;

    [Header("Values to verify placement")] [SerializeField]
    private float _radius;

    [SerializeField] private int _collidersAmount = 50;
    [SerializeField] private LayerMask _layerToHit;

    private Collider[] _hitColliders;

    private void Awake()
    {
        if (_colliders.Length <= 0)
        {
            throw new Exception($"Colliders length can not be less than 0");
        }
    }

    public void Init(Player player)
    {
        _hitColliders = new Collider[_collidersAmount];

        if (_interactables == null)
            throw new Exception("Interactables are null");

        foreach (var interactable in _interactables.Keys)
        {
            int count = _interactables[interactable];

            for (int i = 0; i < count; i++)
            {
                Collider spawnCollider = GetCollider();

                Vector3 position = GetPosition(spawnCollider);

                interactable.Init(player);

                interactable.Spawn(position);

                if (interactable is CostInteractableHandler costInteractable)
                {
                    costInteractable.SetValueForObjects();
                }
            }
        }
    }

    private Vector3 GetPosition(Collider collider)
    {
        float spawnAreaMinX = collider.bounds.min.x;
        float spawnAreaMaxX = collider.bounds.max.x;

        float spawnAreaMinZ = collider.bounds.min.z;
        float spawnAreaMaxZ = collider.bounds.max.z;

        float positionX = Random.Range(spawnAreaMinX, spawnAreaMaxX);
        float positionY = collider.bounds.min.y;
        float positionZ = Random.Range(spawnAreaMinZ, spawnAreaMaxZ);

        return new Vector3(positionX, positionY, positionZ);
    }

    private Collider GetCollider()
    {
        int randomIndex = Random.Range(0, _colliders.Length);

        return _colliders[randomIndex];
    }
}