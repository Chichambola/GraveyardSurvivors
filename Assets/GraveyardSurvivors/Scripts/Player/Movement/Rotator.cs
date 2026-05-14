using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;

    public void Rotate(Vector3 direction)
    {
        if(direction.sqrMagnitude > .01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, _speed * Time.fixedDeltaTime);
        }
    }
}
