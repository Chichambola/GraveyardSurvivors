using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValueViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _valueDiscription;

    private float _value;
    
    private void Start()
    {
        _text.text = _valueDiscription;
    }

    public void ShowValue()
    {
        Debug.Log($"{_text.text}: {_value}");
    }

    public void SetVisibility(bool isShowing)
    {
        _text.enabled = isShowing;
    }

    public void SetValue(float value)
    {
        _value = value;
    }
}
