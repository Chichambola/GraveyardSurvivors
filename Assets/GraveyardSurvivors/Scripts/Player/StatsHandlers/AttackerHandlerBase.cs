using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public abstract class AttackerHandlerBase<T> : Stats<T> where T : IStat
{
    public abstract void SetWeapon(IWeapon weapon);
    public abstract void StartAttacking();
    public abstract void OnEnemyDetected(IAttacker attacker);
    public abstract override void UpdateStats(T stats);
    protected abstract IEnumerator AttackingCoroutine();
}
