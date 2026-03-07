using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticCurvePoints : MonoBehaviour
{
    public void SetPositions(Transform aPoint, Transform bPoint, Transform cPoint)
    {
        APoint = aPoint;
        BPoint = bPoint;
        CPoint = cPoint;
    }

    public Transform APoint{ get; private set; }
    public Transform BPoint { get; private set; }
    public Transform CPoint { get; private set; }
}
