using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : Spawner<Coin>
{
    [SerializeField] private Vector3 _spawnOffset;
    
    private Vector3 _spawnPosition;
    
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
    
    protected override void ActionOnGet(Coin coin)
    {
        coin.CanBeReleased += Release;
        
        float yRotation = UserUtils.GetRandomRotation();
        
        coin.transform.parent = transform;
        coin.transform.position = _spawnPosition;
        coin.transform.rotation = Quaternion.Euler(coin.transform.rotation.x, yRotation, coin.transform.rotation.z);
        
        base.ActionOnGet(coin);
        
        coin.StartMoving();
    }

    protected override void ActionOnRelease(Coin coin)
    {
        coin.CanBeReleased -= Release;
        
        base.ActionOnRelease(coin);
    }
}
