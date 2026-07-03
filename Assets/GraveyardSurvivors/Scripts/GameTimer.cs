using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _debugElapsedTime;
    
    private int _seconds;
    private int _minutes;
    private float _elapsedTime;

    public int Minutes => _minutes;
    
    private void Awake()
    {
        _elapsedTime = _debugElapsedTime;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void OnDisable()
    {
        _elapsedTime = 0;
    }
    
    private void UpdateTimer()
    {
        _elapsedTime += Time.deltaTime;
        
        _minutes = Mathf.FloorToInt(_elapsedTime / 60);
        _seconds = Mathf.FloorToInt(_elapsedTime % 60);
        
        _timerText.text = $"{_minutes:00} : {_seconds:00}";
    }
}
