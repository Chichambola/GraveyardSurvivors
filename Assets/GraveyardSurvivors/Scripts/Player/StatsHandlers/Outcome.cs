using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Outcome : MonoBehaviour, IPoolable<Outcome>
{
    public event Action<Outcome> CanBeReleased;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _sprite.sprite = null;
    }

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void ResetCharacteristics()
    {
        _sprite.sprite = null;
    }

    public void Release()
    {
        CanBeReleased?.Invoke(this);
    }
    
    public void Set(Sprite sprite)
    {
        _sprite.sprite = sprite;
    }
}