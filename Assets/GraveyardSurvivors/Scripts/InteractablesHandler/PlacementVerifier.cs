using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlacementVerifier : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private int _collidersAmount = 100;
    [SerializeField] private LayerMask _layerMask;
    
    private Collider[] _hitColliders;

    private void Awake()
    {
        _hitColliders = new Collider[_collidersAmount];
    }

    public bool IsPlacementValid(Vector3 position)
    {
        int hits = Physics.OverlapSphereNonAlloc(position, _radius, _hitColliders);
        
        for (int i = 0; i < hits; i++)
        {
            if (_hitColliders[i].TryGetComponent(out Interactable _))
            {
                Debug.Log("Placement not verified");
                
                return false;
            }
        }
        
        return true;
    }
}
