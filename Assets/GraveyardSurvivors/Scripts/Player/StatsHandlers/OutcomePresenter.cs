using System;
using Sherbert.Framework.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Serialization;

public class OutcomePresenter : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<EOutcome, Sprite> _outcomes;
    [SerializeField] private OutcomeSpawner _spawner;

    public void Show(EOutcome defenseOutcome)
    {
        if (!_outcomes.Contains(defenseOutcome))
        {
            throw new Exception($"You haven't added {defenseOutcome} to dictionary.");
        }

        _spawner.Spawn(_outcomes[defenseOutcome]);
    }
}