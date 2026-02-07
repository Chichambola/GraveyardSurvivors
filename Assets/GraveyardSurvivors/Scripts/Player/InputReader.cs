using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

public class InputReader : MonoBehaviour
{
    private PlayerInput _playerInput;

    public Vector3 MovementDirection { get; private set; }

    private void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        MovementDirection = _playerInput.Movement.Move.ReadValue<Vector3>();
    }
}
