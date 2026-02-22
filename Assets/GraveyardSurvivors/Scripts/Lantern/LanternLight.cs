using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LanternLight : MonoBehaviour
{
    [SerializeField] private float _shrinkRate = 0.1f;
    
    private SphereCollider _collider;
    private Coroutine _coroutine;
    
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ShrinkingCoroutine());
    }

    private IEnumerator ShrinkingCoroutine()
    {
        while (enabled)
        {
            _collider.radius  = Mathf.Lerp(_collider.radius, 0, Time.deltaTime * _shrinkRate);
            
            yield return null;
        }
    }
}
