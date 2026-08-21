using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickablesSpawner : Spawner<Pickable>, IHandler
{
    [SerializeField] private Vector3 _spawnOffset;
    
    private Vector3 _spawnPosition;
    private IPlayer _player;
    private readonly int _minRotation = 0;
    private readonly int _highestRotation = 360;
    
    public void Init(IPlayer player)
    {
        _player = player ?? throw new Exception("Player is null");
    }
    
    public void Spawn(Vector3 position, float amount)
    {
        if (amount <= 0)
            throw new Exception($"Invalid amount of coins: {amount}.");

        _spawnPosition = position;
        
        for (int i = 0; i < amount; i++)
        {
            GetObject();
        }
    }
    
    protected override void ActionOnGet(Pickable pickable)
    {
        pickable.CanBeReleased += Release;
        pickable.PickedUp += OnPickedUp;
        
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
    
    private void OnPickedUp(Pickable pickable)
    {
        pickable.StartMoving(_player as ITarget).Forget();
    }
}
