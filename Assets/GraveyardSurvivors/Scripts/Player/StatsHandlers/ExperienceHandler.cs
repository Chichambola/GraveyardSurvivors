using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceHandler : MonoBehaviour
{
    [SerializeField] private Player _player;

    private float _xp;
    
    public void GainExperience(float experience)
    {
        _xp += _player.CurrentStats.XpMultiplier * experience;
    }
}
