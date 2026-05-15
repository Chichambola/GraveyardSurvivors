using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    public void Rotate(Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);
        
        if(horizontalDirection.sqrMagnitude > .01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(horizontalDirection);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _speed * Time.fixedDeltaTime);
        }
    }
}
