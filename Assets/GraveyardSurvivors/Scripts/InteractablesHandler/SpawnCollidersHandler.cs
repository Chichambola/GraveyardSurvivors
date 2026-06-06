using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCollidersHandler : MonoBehaviour
{
    [SerializeField] private List<Collider> _colliders;
    
    private void Awake()
    {
        if (_colliders.Count <= 0)
        {
            throw new Exception($"Colliders length can not be less than 0");
        }
    }
    
    public Vector3 GetRandomPosition()
    {
        Collider collider = GetCollider();
        
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
