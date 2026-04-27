using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IPoolable<T>
{
    [SerializeField] protected T ObjectPrefab;
    [SerializeField] protected int PoolCapacity;
    [SerializeField] protected int MaxPoolCapacity;

    private ObjectPool<T> _pool;
    protected readonly List<T> ActiveObjects = new();
    
    public int Count => ActiveObjects.Count;
    
    private void Awake()
    {
        _pool = new ObjectPool<T>
        (
            createFunc: CreateObject,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease,
            actionOnDestroy: @object => Destroy(@object.gameObject),
            collectionCheck: true,
            defaultCapacity: PoolCapacity,
            maxSize: MaxPoolCapacity);
    }

    public void SetPrefab(T objectToSpawn)
    {
        ObjectPrefab = objectToSpawn;
    }
    
    public void ReleaseAll() 
    {
        if (ActiveObjects.Count != 0)
        {
            foreach (T obj in ActiveObjects.ToList())
            {
                Release(obj);
            }

            ActiveObjects.Clear();
        }
    }

    protected T GetObject()
    {
        var @object = _pool.Get();

        return @object;
    }
    
    protected virtual void ActionOnGet(T @object)
    {
        @object.gameObject.SetActive(true);
    }

    protected void Release(T @object)
    {
        if(@object.gameObject.activeSelf)
        {
            _pool.Release( @object );
        }
    }

    protected virtual void ActionOnRelease(T @object)
    {
        @object.gameObject.SetActive(false);
    }

    private T CreateObject()
    {
        return Instantiate(ObjectPrefab);
    }
}