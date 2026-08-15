using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScreenControls : MonoBehaviour
{
    [SerializeField] private Button _movementButton;
    [SerializeField] private Button _stopButton;
    [SerializeField] private InputReader _inputReader;

    private bool _wasPressed;
    private Vector3 _direction;

    private void OnEnable()
    {
        _movementButton.onClick.AddListener(ChangeDirection);
        _stopButton.onClick.AddListener(Stop);
    }

    private void OnDisable()
    {
        _movementButton.onClick.RemoveListener(ChangeDirection);
        _stopButton.onClick.RemoveListener(Stop);
    }

    private void Update()
    {
        _inputReader.SetMovementDirection(_direction);
    }

    private void Stop()
    {
        _direction = Vector3.zero;
    }
    
    private void ChangeDirection()
    {
        if (!_wasPressed)
        {
            _direction = new Vector3(1,0,0);
            _wasPressed = true;
        }
        else
        {
            _direction = new Vector3(-1,0,0);
            _wasPressed = false;
        }
    }
}
