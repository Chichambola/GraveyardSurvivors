using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Characters/New enemy")]
public class EnemyInfo : ScriptableObject
{
    [SerializeField] private EnemyStats _stats;
    
    public EnemyStats GetStats()
    {
        return new EnemyStats(_stats);
    }
}
