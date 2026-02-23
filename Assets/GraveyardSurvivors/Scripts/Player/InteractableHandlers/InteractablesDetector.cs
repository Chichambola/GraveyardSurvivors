using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(SphereCollider))]
public class InteractablesDetector : MonoBehaviour
{
    [SerializeField] private int _numberOfColliders = 10;

    private SphereCollider _collider;
    private Collider[] _hitColliders;
    private IInteractable _nearestInteractable;
    private List<IInteractable> _nearbyInteractables;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _hitColliders = new Collider[_numberOfColliders];
        _nearbyInteractables = new List<IInteractable>();
    }

    private void OnValidate()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void Update()
    {
        FindInteractables();
    }

    public bool TryGetInteractable(out IInteractable interactable)
    {
        if (_nearestInteractable != null)
        {
            interactable = _nearestInteractable;
            
            return true;
        }

        interactable = null;
        
        return false;
    }
    
    private void FindInteractables()
    {
        var hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, _collider.radius, _hitColliders);

        float minDistance = float.MaxValue;
        int count = 0;

        for (int i = 0; i < hits; i++)
        {
            if (_hitColliders[i].TryGetComponent(out IInteractable interactable))
            {
                count++;

                float distance = Vector3.Distance(gameObject.transform.position, _hitColliders[i].transform.position);

                if (distance < minDistance && interactable.IsCurrentlyAvailable)
                {
                    minDistance = distance;

                    _nearestInteractable = interactable;
                }

                _nearbyInteractables.Add(interactable);
            }
        }
        
        DefineHighlightStatus(count);
    }

    private void DefineHighlightStatus(int count)
    {
        if (count == 0 && _nearbyInteractables.Count != 0)
        {
            foreach (var interactable in _nearbyInteractables)
            {
                if(interactable.IsCurrentlyShowingValue)
                    interactable.HideValue();
                
                interactable.ChangeOutlineVisibility(false);
            }

            _nearestInteractable = null;
        }
        else
        {
            foreach (var interactable in _nearbyInteractables)
            {
                if (interactable != _nearestInteractable)
                {
                    if (interactable.IsCurrentlyShowingValue)
                    {
                        interactable.HideValue();
                    }
                    
                    interactable.ChangeOutlineVisibility(false);  
                }
                else if (_nearestInteractable.IsCurrentlyAvailable)
                {
                    if (_nearestInteractable.IsCurrentlyShowingValue == false)
                    {
                        _nearestInteractable.ShowValue();
                    }
                    
                    _nearestInteractable.ChangeOutlineVisibility(true);
                }
            }
        }
    }
}