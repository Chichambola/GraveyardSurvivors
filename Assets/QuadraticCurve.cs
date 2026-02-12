using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        
        return Vector3.Lerp(fromAToControl, fromControlToB, time);
    }

    public void SetPointsPosition(QuadraticCurvePoints curvePoints)
    {
        _aPoint = curvePoints.APoint.position;
        _bPoint = curvePoints.BPoint.position;
        _controlPoint = curvePoints.ControlPoint.position;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(Evaluate(i/20f),0.1f);;
        }
    }
}
