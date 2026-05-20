using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickablesSpawner : Spawner<Pickable>
{
    [SerializeField] private Vector3 _spawnOffset;
    
    private Vector3 _spawnPosition;
    private readonly int _minRotation = 0;
    private readonly int _highestRotation = 360;
    
    public void Spawn(Vector3 position, float coinsAmount)
    {
        if (coinsAmount <= 0)
            throw new Exception($"Invalid amount of coins: {coinsAmount}.");

        _spawnPosition = position;
        
        for (int i = 0; i < coinsAmount; i++)
        {
            GetObject();
        }
    }
    
    protected override void ActionOnGet(Pickable pickable)
    {
        pickable.CanBeReleased += Release;
        
        float yRotation = Random.Range(_minRotation, _highestRotation);
        
        pickable.transform.parent = transform;
        pickable.transform.position = _spawnPosition;
        pickable.transform.rotation = Quaternion.Euler(pickable.transform.rotation.x, yRotation, pickable.transform.rotation.z);
        
        base.ActionOnGet(pickable);
        
        pickable.StartMoving();
    }

    protected override void ActionOnRelease(Pickable pickable)
    {
        pickable.CanBeReleased -= Release;
        
        base.ActionOnRelease(pickable);
    }
}
