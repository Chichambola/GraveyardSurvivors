using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class HealAltarHandler : MonoBehaviour, IPriceOwner, IInteractableHandler
{
    [SerializeField] private InteractableHandlerBase _interactableHandler;
    [SerializeField] private CostHandler _costHandler;

    private int _altarsWithPlayerAroundCount;
    private List<HealAltar> _altars;
    private ILightCarrier _lightCarrier;
    private IPlayer _player;

    public int InitialCount => _interactableHandler.InitialCount;

    public void Init(IPlayer player)
    {
        _player = player;
        
        if (_player is ILightCarrier lightCarrier)
        {
            _lightCarrier = lightCarrier;
        }
        else
        {
            throw new Exception("Player must extend interface ILightCarrier.");
        }
    }

    private void Awake()
    {
        _altars = new List<HealAltar>();
    }

    private void OnEnable()
    {
        _interactableHandler.HandleChosenObject += OnInteractableChosen;
    }

    private void OnDisable()
    {
        _interactableHandler.HandleChosenObject -= OnInteractableChosen;

        foreach (var altar in _altars)
        {
            altar.DetectedPlayer -= IncreaseCount;
            altar.LostPlayer -= DecreaseCount;
        }
    }

    public void Spawn(Vector3 position)
    {
        var interactable = _interactableHandler.Spawn(position);
        
        if (interactable is HealAltar altar)
        {
            _altars.Add(altar);

            altar.DetectedPlayer += IncreaseCount;
            altar.LostPlayer += DecreaseCount;
        }
    }
    
    public void InitializePrices() => _interactableHandler.SetValueForObjects(_costHandler.Value);
    
    private void OnInteractableChosen<T>(T interactable)
    {
        if (interactable is not HealAltar altar)
            throw new Exception(nameof(altar));
        
        if (_player.MoneyAmount < altar.Value)
        {
            Debug.Log("Not enough money");
            
            return;
        }
        
        _player.ReduceMoney(altar.Value);
        
        altar.IncreaseInteractionAmount();
        
        altar.Set(_costHandler.Increase(altar.Value));
    }
    
    private void IncreaseCount()
    {
        _altarsWithPlayerAroundCount++;
        
        _lightCarrier.PauseLight();
    }
    
    private void DecreaseCount()
    {
        _altarsWithPlayerAroundCount--;

        if (_altarsWithPlayerAroundCount <= 0)
        {
            _lightCarrier.StartLight();
        }
    }
}