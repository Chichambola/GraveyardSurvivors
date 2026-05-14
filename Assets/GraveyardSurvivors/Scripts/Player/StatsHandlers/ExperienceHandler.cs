using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    private float _currentXp;
    private float _targetXp;

    public void Init(float targetXp)
    {
        _targetXp = targetXp;
    }
    
    public void GainExperience(float experience)
    {
        _currentXp += _player.CurrentStats.XpMultiplier * experience;
    }

    public void UpdateTargetXp(float targetXpMultiplier)
    {
        
    }
}
