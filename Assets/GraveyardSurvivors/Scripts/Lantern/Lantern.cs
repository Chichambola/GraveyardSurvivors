using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

public class Lantern : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private float _shrinkRateIncrease = 0.05f;
    [SerializeField] private LanternLight _light;
    [Header("Services")]
    [SerializeField] private LanternDamageDealer _damageDealer;
    [SerializeField] private PlayerDetector _playerDetector;
    
    private float _lastRadius;
    private float _defaultValue = 0f;
    private float _currentShrinkRate;
    private Coroutine _coroutine;
    
    public void Start()
    {
        _light.Init();    
    }
    
    private void OnEnable()
    {
        _damageDealer.EnemyDetected += OnEnemyDetected;
        _damageDealer.EnemyLeft += OnEnemyLeft;
        _damageDealer.EnemyDied += ProcessEnemyDeath;
        _light.ThresholdReached += OnThresholdReached;
        _light.GainedEnergy += OnEnergyGained;
        _playerDetector.PlayerDetected += OnPlayerDetected;
        _playerDetector.PlayerLeft += OnPlayerLeft;
    }

    private void OnDisable()
    {
        _damageDealer.EnemyDetected -= OnEnemyDetected;
        _damageDealer.EnemyLeft -= OnEnemyLeft;
        _damageDealer.EnemyDied -= ProcessEnemyDeath;
        _light.ThresholdReached -= OnThresholdReached;
        _light.GainedEnergy -= OnEnergyGained;
        _playerDetector.PlayerDetected -= OnPlayerDetected;
        _playerDetector.PlayerLeft -= OnPlayerLeft;
    }
    
    public void ProcessEnemyDeath(Enemy enemy)
    {
        if (enemy == null)
            throw new Exception("Enemy cannot be null!");

        if (_light.CurrentRadius > _defaultValue)
        {
            _light.ReceiveEnergy(enemy.CurrentStats.LanternEnergy);
        }
        else
        {
            float tempValue = _lastRadius.AddPercentToNumber(enemy.CurrentStats.LanternEnergy);

            _light.SetLightRadiusForAllAxis(tempValue);

            _light.ChangeState(true);
        }
    }
    
    private void OnPlayerDetected(ILightCarrier carrier)
    {
        if (carrier == null)
            throw new Exception();

        carrier.IncreaseLanternCount();
        
        LanternLight carrierLight = carrier.Light;

        if (carrier.LanternsCount < 0)
        {
            carrierLight.GainedEnergy += OnEnergyGained;
            carrierLight.ThresholdReached += OnThresholdReached;
        }
        
        carrierLight.ResetRadius();

        carrierLight.SetRate(_defaultValue);
    }

    private void OnPlayerLeft(ILightCarrier carrier)
    {
        if (carrier == null)
            throw new Exception();

        carrier.DecreaseLanternCount();

        if (carrier.LanternsCount > 0)
            return;

        LanternLight carrierLight = carrier.Light;

        carrierLight.StartRadiusRoutine(_defaultValue);

        carrierLight.ResetRate();

        if (carrierLight.IsGainingEnergy)
            carrierLight.SetGainingEnergyState(false);

        carrierLight.GainedEnergy -= OnEnergyGained;
        carrierLight.ThresholdReached -= OnThresholdReached;
    }

    private void OnEnemyLeft()
    {
        DecreaseRate();
    }

    private void DecreaseRate()
    {
        float currentRate = _light.ShrinkRate;

        currentRate -= _shrinkRateIncrease;

        if (currentRate < 0)
            currentRate = 0;

        _light.SetRate(currentRate);
    }

    private void OnEnemyDetected()
    {
        IncreaseRate();
    }

    private void IncreaseRate()
    {
        float currentRate = _light.ShrinkRate;

        currentRate += _shrinkRateIncrease;

        _light.SetRate(currentRate);
    }

    private void OnEnergyGained(LanternLight lanternLight)
    {
        if (lanternLight == null)
            throw new Exception("Light can not be null");

        lanternLight.ChangeState(true);
    }

    private void OnThresholdReached(LanternLight lanternLight)
    {
        _lastRadius = lanternLight.CurrentRadius;
    }
}