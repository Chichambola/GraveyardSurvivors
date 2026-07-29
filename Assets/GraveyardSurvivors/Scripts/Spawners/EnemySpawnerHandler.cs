using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnerHandler : MonoBehaviour
{
    [Header("Spawners")] [SerializeField] private List<EnemySpawner> _enemySpawners;
    [SerializeField] private EnemySpawner _lastMinuteSpawner;

    [Header("Services")] [SerializeField] private SpawnCollidersHandler _spawnCollidersHandler;
    [SerializeField] private PlacementVerifier _placementVerifier;

    [SerializeField] private SpawnersPickerSettings _initialValues;

    [Header("Upgrade timing settings")] [SerializeField]
    private List<float> _upgradesPercentsInitialValues;

    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private int _thresholdMinute = 7;

    public event Action<Enemy> EnemyWasKilled;

    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _availableSpawners;
    private Queue<EnemySpawner> _spawnQueue;
    private List<float> _upgradePercents;
    private IPlayer _player;
    private FuncPredicate _canUnsubscribe;
    private CancellationTokenSource _ctsSpawn;
    private CancellationTokenSource _ctsPoints;
    private SpawnersPickerSettings _settings;
    private bool _isGameTimerAttached;
    private bool _isLastMinute;
    private float _availablePoints;

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

        _settings = _initialValues;

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
        _availablePoints = _settings.InitialAvailablePoints;

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
            FillSpawnQueue();

            await Spawn();

            _ctsPoints = new CancellationTokenSource();
            _ctsPoints.RegisterRaiseCancelOnDestroy(gameObject);

            await GainPointsTask();
            
            _ctsPoints.Cancel();
        }
    }

    private async UniTask Spawn()
    {
        foreach (var enemySpawner in _spawnQueue)
        {
            if (_currentEnemies.Count >= _settings.MaxEnemiesAmount)
            {
                await UniTask.WaitUntil(() => _currentEnemies.Count < _settings.MaxEnemiesAmount, cancellationToken: _ctsSpawn.Token);
            }
            else
            {
                Spawn(enemySpawner);
                
                float time = GetSpawnTime();
                    
                await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _ctsSpawn.Token);
            }
        }
    }

    private void FillSpawnQueue()
    {
        _spawnQueue = new Queue<EnemySpawner>();
        
        float sum = 0;
        bool isEnoughPoints = true;

        while (isEnoughPoints)
        {
            var chosenSpawner = UserUtils.GetElementByWeight(_availableSpawners);

            sum += chosenSpawner.Cost;

            if (sum <= _availablePoints)
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
        _availablePoints -= chosenSpawner.Cost;

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
        _settings.Upgrade(percent);
        
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

    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);

        EnemyWasKilled?.Invoke(enemy);
    }

    private async UniTask GainPointsTask()
    {
        float elapsedTime = 0;
        float lastSecond = 0;
        float time = GetSpawnTime();

        while (!_ctsPoints.IsCancellationRequested && elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float seconds = Mathf.FloorToInt(elapsedTime % 60);
            
            if (!Mathf.Approximately(seconds, lastSecond))
            {
                lastSecond = seconds;
                
                _availablePoints += _settings.PointsGainPerSecond;
            }

            await UniTask.Yield(_ctsPoints.Token);
        }

        if (_availablePoints >= _settings.MaxPoints)
        {
            _availablePoints = _settings.MaxPoints;
        }
    }

    private float GetSpawnTime() => Random.Range(_settings.SpawnRateMinTime, _settings.SpawnRateMaxTime);
}