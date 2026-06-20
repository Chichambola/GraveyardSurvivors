using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableHashSet<T> : ISerializationCallbackReceiver
{
    [SerializeField] private List<T> _serializedItems;
    
    private HashSet<T> _uniqueItems = new();
    
    public void OnBeforeSerialize()
    {
        _serializedItems = _uniqueItems.ToList();
    }

    public void OnAfterDeserialize()
    {
        _uniqueItems = new HashSet<T>(_serializedItems);
        _serializedItems = null;
    }
}
