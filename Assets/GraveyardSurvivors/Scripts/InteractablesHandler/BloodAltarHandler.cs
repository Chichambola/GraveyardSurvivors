using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAltarHandler : MonoBehaviour, IInteractableHandler
{
    [SerializeField] private InteractableSpawner _interactableSpawner;
    [SerializeField] private int _initialSpawnCount = 10;
    [SerializeField] private float _divider = 1.25f;
    
    private IPlayer _player;

    public int InitialCount => _initialSpawnCount;

    public void Init(IPlayer player)
    {
        _player = player;
    }
    
    private void OnValidate()
    {
        if (_initialSpawnCount < 0)
        {
            _initialSpawnCount = 0;
        }
    }

    private void OnEnable()
    {
        _interactableSpawner.InteractableWasChosen += OnInteractableChosen;
    }

    private void OnDisable()
    {
        _interactableSpawner.InteractableWasChosen -= OnInteractableChosen;
    }

    public void Spawn(Vector3 position)
    {
        _interactableSpawner.Spawn(position);
    }
    
    private void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not BloodAltar altar)
            throw new Exception(nameof(altar));
        
        float necessaryHealth = GetAmountOfNecessaryHealth(_player.MaxHealth, altar);

        if (_player.CurrentHealth <= necessaryHealth)
        {
            Debug.Log("Not enough health");
            
            return;
        }

        altar.IncreaseInteractionAmount();
        
        altar.StartCountdown();

        float moneyAmount = necessaryHealth / _divider;
        
        _player.TakeDamage(necessaryHealth);
        _player.ReceiveMoney(moneyAmount);
    }
    
    private float GetAmountOfNecessaryHealth(float maxPlayerHealth, BloodAltar altar)
    {
        float maxPercent = 100;
        
        float damagePercent = altar.GetDamagePercent();
        
        float damage = maxPlayerHealth * (damagePercent / maxPercent);
        
        return damage;
    }
}
