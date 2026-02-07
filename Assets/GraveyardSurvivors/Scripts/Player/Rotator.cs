using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(direction);
    }
}
