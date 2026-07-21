using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerDetector : Detector
{
    [SerializeField] private AttackArea _attackArea;
    
    public event Action<IPlayer> PlayerDetected;
    public event Action PlayerLeft;

    private Coroutine _coroutine;
    private float _checkTime = .6f;
    
    public bool IsPlayerNear { get; private set; }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player))
        {
            PlayerDetected?.Invoke(player);

            IsPlayerNear = true;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(CheckRoutine());
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayer _))
        {
            PlayerLeft?.Invoke();

            IsPlayerNear = false;

            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }

    private void OnDisable()
    {
        IsPlayerNear = false;
    }

    private IEnumerator CheckRoutine()
    {
        var wait = new WaitForSeconds(_checkTime);

        bool isPlayerNear = false;
        
        while (IsPlayerNear)
        {
            if (_attackArea.TryGetAttackers(out var attackers))
            {
                foreach (var attacker in attackers)
                {
                    if (attacker is IPlayer)
                    {
                        isPlayerNear = true;
                    }
                }
            }

            if (isPlayerNear == false)
            {
                IsPlayerNear = false;
            }
            
            yield return wait;
        }
    }
}
