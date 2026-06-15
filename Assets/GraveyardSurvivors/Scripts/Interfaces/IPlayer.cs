using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public event Action<Item> PickedItem;
    public Vector3 CurrentPosition { get; }
    public float MaxHealth { get; }
    public float MoneyAmount { get; }
    public float CurrentHealth { get; }
    public float Luck { get; }
    public bool IsLightActive { get; }
    void ReduceMoney(float value);
    void ReceiveMoney (float value);
    void TakeDamage(float value);
    void Heal(float value);
    void ResetRadius();
    void StartLight();
}
