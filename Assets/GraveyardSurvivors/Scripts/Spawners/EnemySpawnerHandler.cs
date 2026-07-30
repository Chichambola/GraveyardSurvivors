using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour
{
    [Header("Spawners")]
    [SerializeField] private List<EnemySpawner> _enemySpawners;

    [Header("Services")]
    [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;

    [SerializeField] private PointsHandler _pointsHandler;
    [SerializeField] private int _maxEnemiesAmount;
    [SerializeField] private float _spawnRate;

    [Header("Upgrade timing settings")]
    [SerializeField] private List<float> _upgradesPercentsInitialValues;
    
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private int _thresholdMinute = 7;

    public event Action<Enemy> EnemyWasKilled;

    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _availableSpawners;
    private List<float> _upgradePercents;
    private IPlayer _player;
    private Queue<EnemySpawner> _spawnQueue;
    private FuncPredicate _canUnsubscribe;
    private CancellationTokenSource _ctsSpawn;
    private bool _isGameTimerAttached;
    private bool _isLastMinute;

    public void Init(IPlayer player)
    {
        if (_enemySpawners.Count <= 0)
            throw new Exception($"Length can not be less than 0");

        _player = player;
    }

    private void Awake()
    {
        _currentEnemies = new List<Enemy>();
        _availableSpawners = new List<EnemySpawner>();
        
        _canUnsubscribe = new FuncPredicate(() => 
            _upgradePercents.Count > 0 || _availableSpawners.Count != _enemySpawners.Count || !_isLastMinute);
        
        _ctsSpawn = new CancellationTokenSource();
        _ctsSpawn.RegisterRaiseCancelOnDestroy(gameObject);
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
        FindAvailableSpawners();

        SpawnTask().Forget();

        if (!_isGameTimerAttached)
            _gameTimer.ReachedMinute += OnMinuteReached;
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

    private async UniTaskVoid SpawnTask()
    {
        while (!_ctsSpawn.IsCancellationRequested)
        {
            FillSpawnQueue(out float sum);

            _pointsHandler.ReducePoints(sum);
            
            await Spawn();

            await _pointsHandler.GainPoints();
        }
    }

    private async UniTask Spawn()
    {
        foreach (var enemySpawner in _spawnQueue)
        {
            if (_currentEnemies.Count >= _maxEnemiesAmount)
            {
                await UniTask.WaitUntil(() => _currentEnemies.Count < _maxEnemiesAmount, cancellationToken: _ctsSpawn.Token);
            }
            else
            {
                Spawn(enemySpawner);
                    
                await UniTask.Delay(TimeSpan.FromSeconds(_spawnRate), cancellationToken: _ctsSpawn.Token);
            }
        }
    }

    private void FillSpawnQueue(out float sum)
    {
        _spawnQueue = new Queue<EnemySpawner>();
        
        sum = 0;
        bool isEnoughPoints = true;

        while (isEnoughPoints)
        {
            var chosenSpawner = UserUtils.GetElementByWeight(_availableSpawners);

            sum += chosenSpawner.Cost;

            if (sum <= _pointsHandler.AvailablePoints)
            {
                _spawnQueue.Enqueue(chosenSpawner);
            }
            else
            {
                isEnoughPoints = false;
            }
        }
    }

    private void Spawn(EnemySpawner chosenSpawner)
    {
        bool canPlace = false;

        while (!canPlace)
        {
            var point = _spawnCollidersHandler.GetRandomPosition();

            if (!_placementVerifier.IsPlacementValid(point))
                continue;

            canPlace = true;

            chosenSpawner.Spawn(point);
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
            AddAvailableSpawner();
        }

        if (minute >= _thresholdMinute && _enemySpawners.Count == 1)
        {
            _isLastMinute = true;
            
            _availableSpawners.Clear();
            
            AddAvailableSpawner();
        }

        if (_canUnsubscribe.Evaluate())
            return;

        _isGameTimerAttached = false;
        _gameTimer.ReachedMinute -= OnMinuteReached;
    }

    private void AddAvailableSpawner()
    {
        var enemySpawner = _enemySpawners.First(spawner => !spawner.IsAvailable);

        enemySpawner.SetActive(true);
        
        _availableSpawners.Add(enemySpawner);
    }

    private void Upgrade(float percent)
    {
        _maxEnemiesAmount = _maxEnemiesAmount.AddPercentToNumber(percent);
        _pointsHandler.Upgrade(percent);
        
        foreach (var enemySpawner in _enemySpawners)
        {
            enemySpawner.Upgrade();
        }
    }
    
    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);

        EnemyWasKilled?.Invoke(enemy);
    }
}