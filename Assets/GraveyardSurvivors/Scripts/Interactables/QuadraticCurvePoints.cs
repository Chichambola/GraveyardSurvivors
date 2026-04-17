using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class QuadraticCurvePoints : MonoBehaviour
{
    [SerializeField] private Transform _aPoint;
    [SerializeField] private Transform _bPoint;
    [SerializeField] private Transform _cPoint;
    [SerializeField] private bool _setOnAwake = true;

    public Vector3 APoint { get; private set; }
    public Vector3 BPoint { get; private set; }
    public Vector3 CPoint { get; private set; }

    private void OnEnable()
    {
        if (_setOnAwake)
        {
            SetPositions(_aPoint.position, _bPoint.position, _cPoint.position);
        }
    }

    public void SetPositions(Vector3 aPoint, Vector3 bPoint, Vector3 cPoint)
    {
        APoint = aPoint;
        BPoint = bPoint;
        CPoint = cPoint;
    }
}
