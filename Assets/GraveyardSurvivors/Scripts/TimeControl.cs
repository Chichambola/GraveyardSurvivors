using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _slider.maxValue = 1;
        _slider.minValue = 0;
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(value => Time.timeScale = value);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(value => Time.timeScale = value);
    }
}
