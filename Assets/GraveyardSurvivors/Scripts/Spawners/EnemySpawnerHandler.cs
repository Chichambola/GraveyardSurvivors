using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
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
    
    [SerializeField] private SpawnersPickerSettings _initialValues;
    
    [Header("Upgrade timing settings")]
    [SerializeField] private List<float> _upgradesPercentsInitialValues;
    [SerializeField] private GameTimer _gameTimer;
    [SerializeField] private int _thresholdMinute = 7;

    public event Action<Enemy> EnemyWasKilled;
    
    private Coroutine _spawnRoutine;
    private List<Enemy> _currentEnemies;
    private List<EnemySpawner> _availableSpawners;
    private List<float> _upgradePercents;
    private IPlayer _player;
    private FuncPredicate _canUnsubscribe;
    private CancellationTokenSource _ctsSpawn;
    private CancellationTokenSource _ctsPoints;
    private SpawnersPickerSettings _settings;
    private bool _isChoosing;
    private bool _isGameTimerAttached;
    private bool _isLastMinute;
    private int _availablePoints;

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
        _canUnsubscribe = new FuncPredicate(() => _upgradePercents.Count > 0 || _availableSpawners.Count != _enemySpawners.Count || !_isLastMinute);
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
            var time = Random.Range(_settings.SpawnRateMinTime, _settings.SpawnRateMaxTime);
            
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: _ctsSpawn.Token);
            
            var chosenSpawner = UserUtils.GetElementByWeight(_availableSpawners);
            
            if (_availablePoints > chosenSpawner.Cost && _currentEnemies.Count < _settings.MaxEnemiesAmount)
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
            else
            {
                _ctsPoints = new CancellationTokenSource();
                
                _ctsPoints.RegisterRaiseCancelOnDestroy(gameObject);
                
                time = Random.Range(_settings.SpawnRateMinTime, _settings.SpawnRateMaxTime);
                
                await GainPointsTask(time);
                
                _ctsPoints.Cancel();
            }
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
    
    private void OnEnemyRelease(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);

        EnemyWasKilled?.Invoke(enemy);
    }

    private async UniTask GainPointsTask(float time)
    {
        float elapsedTime = 0;

        while (!_ctsPoints.IsCancellationRequested || !Mathf.Approximately(elapsedTime, time))
        {
            elapsedTime += Time.deltaTime;
            
            _availablePoints = _availablePoints.AddPercentToNumber(_settings.PointGainPercent);

            if (!(_availablePoints > _settings.MaxPoints))
                return;
            
            _availablePoints = _settings.MaxPoints;

            await UniTask.Yield(PlayerLoopTiming.Update, _ctsPoints.Token);
        }
    }
}