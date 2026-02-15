using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public float MaxHealth { get; }
    bool HasEnoughHealth(float value);
    bool HasEnoughMoney(float value);
    void ReduceMoneyAmount(float value);
    void ReceiveMoney (float value);
    void TakeDamage(float value);
}
