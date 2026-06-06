using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public Vector3 CurrentPosition { get; }
    public float MaxHealth { get; }
    public float MoneyAmount { get; }
    public float CurrentHealth { get; }
    public float Luck { get; }
    public bool IsLightActive { get; }
    void ReduceMoney(float value);
    void ReceiveMoney (float value);
    void TakeDamage(float value);
}
