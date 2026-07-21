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
    public event Action<int> ReachedMinute;
    
    private int _seconds;
    private int _minutes;
    private int _previousMinute;
    private float _elapsedTime;
    
    public static GameTimer Instance { get; private set; }
    
    private void Awake()
    {
        _elapsedTime = _debugElapsedTime;
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void OnEnable()
    {
        _previousMinute = 0;
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
        
        if (_minutes != _previousMinute)
        {
            ReachedMinute?.Invoke(_minutes);
            
            _previousMinute = _minutes;
        }
        
        _timerText.text = $"{_minutes:00} : {_seconds:00}";
    }
}
