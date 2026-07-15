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
    public int Seconds => _seconds;
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

    private void OnDisable()
    {
        _elapsedTime = 0;
    }
    
    private void UpdateTimer()
    {
        _elapsedTime += Time.deltaTime;
        
        _minutes = Mathf.FloorToInt(_elapsedTime / 60);
        Debug.Log(_minutes);
        _seconds = Mathf.FloorToInt(_elapsedTime % 60);
        
        _timerText.text = $"{_minutes:00} : {_seconds:00}";
    }
}
