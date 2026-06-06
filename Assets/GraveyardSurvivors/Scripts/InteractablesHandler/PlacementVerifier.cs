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
    [SerializeField] private Vector3 _size;
    [SerializeField] private int _collidersAmount = 100;
    [SerializeField] private LayerMask _layerMask;
    
    private Collider[] _hitColliders;

    private void Awake()
    {
        _hitColliders = new Collider[_collidersAmount];
    }

    public bool IsPlacementValid(Vector3 position)
    {
       position = new Vector3(position.x, position.y +3 , position.z);
       
        if (Physics.CheckBox(position, _size , Quaternion.identity,_layerMask))
        {
            Debug.Log("Placement not verified");

            return false;
        }

        return true;
        
        //int hits = Physics.OverlapSphereNonAlloc(position, _radius, _hitColliders);

        int hits = Physics.OverlapSphereNonAlloc(position, _radius, _hitColliders);

        string objects = String.Empty;

        for (int i = 0; i < hits; i++)
        {
            objects += $"\n{_hitColliders[i].name} \n";
        }
        
        Debug.Log(objects);
        
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
