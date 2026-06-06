using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;

public class PlacementVerifier : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;

    public bool IsPlacementValid(Vector3 position) => !Physics.CheckSphere(position, _radius, _layerMask);
}
