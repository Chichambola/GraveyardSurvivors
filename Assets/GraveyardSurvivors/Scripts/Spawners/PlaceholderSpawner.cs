using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderSpawner : Spawner<ItemPlaceholder>
{
    [SerializeField] private Thrower _thrower;

    public event Action<Vector3> ItemStoppedMoving;
    
    private QuadraticCurvePoints _curvePoints;
    private ItemPlaceholder _currentPlaceholder;

    private void OnEnable()
    {
        _thrower.FinishedMoving += OnFinishedMoving;
    }

    private void OnDisable()
    {
        _thrower.FinishedMoving -= OnFinishedMoving;
    }

    public void Spawn(QuadraticCurvePoints curvePoints)
    {
        _curvePoints = curvePoints;
        
        GetObject();
    }
    
    protected override void ActionOnGet(ItemPlaceholder @object)
    {
        @object.transform.parent = transform;
        
        base.ActionOnGet(@object);

        @object.CanBeReleased += Release;

        _currentPlaceholder = @object; 
        
        _thrower.StartThrowing(@object, _curvePoints);
    }

    protected override void ActionOnRelease(ItemPlaceholder @object)
    {
        base.ActionOnRelease(@object);

        @object.CanBeReleased -= Release;
    }
    
    private void OnFinishedMoving()
    {
        ItemStoppedMoving?.Invoke(_currentPlaceholder.Rigidbody.position);
        
        _currentPlaceholder.Release();
    }
}
