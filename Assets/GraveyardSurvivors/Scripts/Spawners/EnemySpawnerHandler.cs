using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerHandler : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] _enemySpawners;

    public event Action<Enemy> EnemyWasKilled;
    
    private Coroutine _choosingRoutine;
    private EnemySpawner _chosenSpawner;
    private Dictionary<EnemySpawner, int> _spawnersWeights;
    private int _availablePoints;

    private void Awake()
    {
        _spawnersWeights = new Dictionary<EnemySpawner, int>();
    }

    private void OnEnable()
    {
        _availablePoints = 100;
        
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
        }

        foreach (var enemySpawner in _enemySpawners)
        {
            _spawnersWeights.Add(enemySpawner, enemySpawner.Weight);
        }
    }

    private void OnDisable()
    {
        foreach (IEnemySpawner<Enemy> enemySpawner in _enemySpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
        }
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        EnemyWasKilled?.Invoke(enemy);
    }

    private IEnumerator ChoosingRoutine()
    {
        while (enabled)
        {
            yield return null;
        }
    }
}
