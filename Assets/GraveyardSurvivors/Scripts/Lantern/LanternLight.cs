using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LanternLight : MonoBehaviour
{
    [SerializeField] private float _shrinkRate = 0.1f;
    [SerializeField] private ParticleSystem _lightArea;
    [SerializeField] private float _initialLightAreaScale = 2f;
    [SerializeField] private Light _light;
    
    private SphereCollider _collider;
    private Coroutine _coroutine;
    
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        var particleSize = new Vector3(_initialLightAreaScale, _initialLightAreaScale, _initialLightAreaScale);

        _lightArea.transform.localScale = particleSize;
        
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        StartCoroutine(ShrinkingCoroutine());
    }

    private IEnumerator ShrinkingCoroutine()
    {
        while (enabled)
        {
            float shrinkRate = Time.deltaTime * _shrinkRate;
            
            _collider.radius  = Mathf.Lerp(_collider.radius, 0, shrinkRate);

            _light.range = Mathf.Lerp(_light.range, 0, shrinkRate);
            
            var particleSize = new Vector3(_collider.radius, _collider.radius, _collider.radius);
            
            _lightArea.transform.localScale = particleSize;
            
            yield return null;
        }
    }
}
