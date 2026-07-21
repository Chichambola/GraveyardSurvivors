using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sherbert.Framework.Generic;
using Unity.Collections;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private EnemySpawner _lastMinuteSpawner;
    
    [Header("Services")]
    [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;
    
    [Header("Settings")]
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
    private int _thresholdMinute = 7;
    private bool _isChoosing;
    private bool _isGameTimerAttached;
    private bool _isLastMinute;
    private FuncPredicate _canUnsubscribe;

    public void Init(IPlayer player)
    {
        if (_enemySpawners.Count <= 0)
            throw new Exception($"Length can not be less than 0");

        _player = player;
    }
    
    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        _enemiesToSpawn = new Queue<EnemySpawner>();
        _availableSpawners = new List<EnemySpawner>();
        _canUnsubscribe = new FuncPredicate(() => _upgradePercents.Count > 0 || _availableSpawners.Count != _enemySpawners.Count || !_isLastMinute);
    }
    
    private void OnEnable()
    {
        _isGameTimerAttached = true;

        _isLastMinute = false;
        
        _upgradePercents = _upgradesPercentsInitialValues.ToList();

        _gameTimer.ReachedMinute += OnMinuteReached;
    }

    private void OnDisable()
    {
        foreach (var enemySpawner in _availableSpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
            enemySpawner.EnemyWasSpawned -= _currentEnemies.Add;
        }

        _currentEnemies.Clear();
        _availableSpawners.Clear();

        if (_isGameTimerAttached)
            _gameTimer.ReachedMinute -= OnMinuteReached;
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

        if (!(_availablePoints > _settings.MaxPoints))
            return;
        
        _availablePoints = _settings.MaxPoints;
            
        OnTimerStopped();
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
    
    private void OnMinuteReached(int minute)
    {
        if (_upgradePercents.Count > 0)
        {
            var percent = _upgradePercents.First();
            
            _upgradePercents.Remove(percent);
            
            Upgrade(percent);
        }

        if (_availableSpawners.Count != _enemySpawners.Count)
        {
            var enemySpawner = _enemySpawners.First(spawner => !spawner.IsAvailable);
            
            enemySpawner.SetActive(true);
            
            _availableSpawners.Add(enemySpawner);
        }

        if (minute >= _thresholdMinute)
        {
            _isLastMinute = true;

            SetLastMinuteSpawner();
        }
        
        if (_canUnsubscribe.Evaluate())
            return;
        
        _isGameTimerAttached = false;
        _gameTimer.ReachedMinute -= OnMinuteReached;
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
    
    private void SetLastMinuteSpawner()
    {
        foreach (var enemySpawner in _availableSpawners)
        {
            enemySpawner.EnemyWasReleased -= OnEnemyRelease;
            enemySpawner.EnemyWasSpawned -= _currentEnemies.Add;
        }
            
        _availableSpawners.Clear();
            
        _availableSpawners.Add(_lastMinuteSpawner);
        _lastMinuteSpawner.SetPlayer(_player);
        _lastMinuteSpawner.SetActive(true);
        _lastMinuteSpawner.EnemyWasSpawned += _currentEnemies.Add;
        _lastMinuteSpawner.EnemyWasReleased += OnEnemyRelease;
    }
}