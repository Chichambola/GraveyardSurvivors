using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class QuadraticCurve : MonoBehaviour
{
    private Vector3 _aPoint;
    private Vector3 _bPoint;
    private Vector3 _controlPoint;

    public Vector3 Evaluate(float time)
    {
        Vector3 fromAToControl = Vector3.Lerp(_aPoint, _controlPoint, time);
        Vector3 fromControlToB = Vector3.Lerp(_controlPoint, _bPoint, time);
        Vector3 fromAtoB = Vector3.Lerp(fromAToControl, fromControlToB, time);
        
        return fromAtoB;
    }

    public void SetPointsPosition(QuadraticCurvePoints curvePoints)
    {
        _aPoint = curvePoints.APoint;
        _bPoint = curvePoints.BPoint;
        _controlPoint = curvePoints.CPoint;
    }
}
