using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour, IRestarter
{
    [Header("Spawners settings")]
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;

    [SerializeField] private SpawnerHandlerSettings _initialValues;
    
    [Header("Upgrade timing settings")]
    [SerializeField] private List<float> _upgradesPercentsInitialValues;
    [SerializeField] private GameTimer _gameTimer;

    public event Action<Enemy> EnemyWasKilled;

    private Coroutine _choosingRoutine;
    private Coroutine _spawnRoutine;
    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _availableSpawners;
    private List<float> _upgradePercents;
    private Queue<EnemySpawner> _enemiesToSpawn;
    private IntervalTimer _timer;
    private IPlayer _player;
    private SpawnerHandlerSettings _settings;
    private float _availablePoints;
    private bool _isChoosing;
    private bool _isGameTimerAttached;

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        _enemiesToSpawn = new Queue<EnemySpawner>();
        _availableSpawners = new List<EnemySpawner>();
        _upgradePercents = _upgradesPercentsInitialValues.ToList();
    }

    private void OnEnable()
    {
        RestartersHandler.Register(this);
        
        _isGameTimerAttached = true;

        _gameTimer.ReachedMinute += OnMinuteReached;
    }

    private void OnDisable()
    {
        UnsubscribeFromSpawners();

        if (_isGameTimerAttached)
            _gameTimer.ReachedMinute -= OnMinuteReached;
        
        RestartersHandler.Deregister(this);
    }

    public void Init(IPlayer player)
    {
        if (_enemySpawners.Count <= 0)
            throw new Exception($"Length can not be less than 0");

        _player = player;
    }
    
    public void Restart()
    {
        UnsubscribeFromSpawners();
        
        StartProcess();
    }

    public void StartProcess()
    {
        _settings = _initialValues;
        
        _availablePoints = _settings.InitialAvailablePoints;
        
        FindAvailableSpawners();
        
        StartChoosing();
        
        if (!_isGameTimerAttached)
            _gameTimer.ReachedMinute += OnMinuteReached;
        
        if (_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);
        
        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }
    
    private void FindAvailableSpawners()
    {
        foreach (var enemySpawner in _enemySpawners)
        {
            enemySpawner.SetPlayer(_player);
            
            if (!enemySpawner.IsAvailable)
                continue;
            
            enemySpawner.EnemyWasReleased += OnEnemyRelease;
            enemySpawner.EnemyWasSpawned += _currentEnemies.Add;
            
            _availableSpawners.Add(enemySpawner);
        }

        if (_availableSpawners.Count <= 0)
            throw new Exception("You need at lease have 1 available spawner at the beginning.");
    }

    private void StartChoosing()
    {
        _isChoosing = true;
        
        if (_choosingRoutine != null)
            StopCoroutine(_choosingRoutine);

        _choosingRoutine = StartCoroutine(ChoosingRoutine());
    }
    
    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);

        EnemyWasKilled?.Invoke(enemy);
    }

    private void OnIntervalReached()
    {
        _availablePoints = _availablePoints.AddPercentToNumber(_settings.PointGainPercent);

        if (_availablePoints > _settings.MaxPoints)
        {
            _availablePoints = _settings.MaxPoints;
            
            OnTimerStopped();
        }
    }

    private void OnTimerStopped()
    {
        _timer.Stopped -= OnTimerStopped;
        _timer.IntervalReached -= OnIntervalReached;

        _timer?.Stop();
        
        StartChoosing();
    }

    private IEnumerator SpawnRoutine()
    {
        var wait = new WaitForSeconds(_settings.SpawnRate);

        while (enabled)
        {
            if (_enemiesToSpawn.Count != 0)
            {
                bool canPlace = false;
                
                var spawner = _enemiesToSpawn.Dequeue();

                while (!canPlace)
                {
                    var point = _spawnCollidersHandler.GetRandomPosition();

                    if (!_placementVerifier.IsPlacementValid(point))
                        continue;
                    
                    canPlace = true;
                    
                    spawner.Spawn(point);
                }
                
                yield return wait;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator ChoosingRoutine()
    {
        var wait = new WaitForSecondsRealtime(_settings.SpawnRate);
        
        while (_isChoosing && _currentEnemies.Count < _settings.MaxEnemiesAmount)
        {
            ChooseSpawner();

            yield return wait;
        }
        
        float time = Random.Range(_settings.MinTime, _settings.MaxTime);
        
        _timer = new IntervalTimer(time);
        _timer.Stopped += OnTimerStopped;
        _timer.IntervalReached += OnIntervalReached;
        _timer.Start();
    }

    private void ChooseSpawner()
    {
        var chosenSpawner = UserUtils.GetElementByWeight(_availableSpawners);

        if (_availablePoints > chosenSpawner.Cost && _currentEnemies.Count < _settings.MaxEnemiesAmount)
        {
            _availablePoints -= chosenSpawner.Cost;

            _enemiesToSpawn.Enqueue(chosenSpawner);
        }
        else
        {
            _isChoosing = false;
        }
    }
    
    private void OnMinuteReached()
    {
        if (_upgradePercents.Count > 0)
        {
            var percent = _upgradePercents.First();
            
            _upgradePercents.Remove(percent);
            
            Upgrade(percent);
        }

        if (_availableSpawners.Count != _enemySpawners.Count)
        {
            var enemySpawner = _enemySpawners.First();
            
            enemySpawner.SetActive(true);
            
            _availableSpawners.Add(enemySpawner);
        }
        
        if (_upgradePercents.Count <= 0 && _enemySpawners.Count == _availableSpawners.Count)
        {
            _isGameTimerAttached = false;
            _gameTimer.ReachedMinute -= OnMinuteReached;
        }
    }
    
    private void Upgrade(float percent)
    {
        var spawnRatePercent = _settings.SpawnRate.GetClampedValueInverse(percent);

        _settings.SpawnRate = _settings.SpawnRate.SubtractPercentFromNumber(spawnRatePercent);
        
        _settings.MaxPoints = _settings.MaxPoints.AddPercentToNumber(percent);
        _settings.MaxEnemiesAmount = _settings.MaxEnemiesAmount.AddPercentToNumber(percent);

        foreach (var enemySpawner in _enemySpawners)
        {
            enemySpawner.Upgrade();
        }
    }
    
    private void UnsubscribeFromSpawners()
    {
        for (int i = _availableSpawners.Count - 1; i >= 0; i--)
        {
            _availableSpawners[i].EnemyWasReleased -= OnEnemyRelease;
            _availableSpawners[i].EnemyWasSpawned -= _currentEnemies.Add;
            
            if (i == 0)
            {
                continue;
            }
            
            _availableSpawners[i].SetActive(false);
        }

        _availableSpawners.Clear();
    }
}