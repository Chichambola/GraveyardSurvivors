using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public Vector3 CurrentPosition { get; }
    public Transform Transform { get; }
    public float MaxHealth { get; }
    public float MoneyAmount { get; }
    public float CurrentHealth { get; }
    public float Luck { get; }
    void ReduceMoneyAmount(float value);
    void ReceiveMoney (float value);
    void TakeDamage(float value);
}
