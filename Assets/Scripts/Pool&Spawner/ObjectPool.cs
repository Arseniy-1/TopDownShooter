using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class ObjectPool<T> where T : CollectableObject
{
    [SerializeField] private int _poolCapacity;
    [SerializeField] private T _objectPrefab;

    private Queue<T> _objects = new Queue<T>();

    private ObjectPool()
    {
        for (int i = 0; i < _poolCapacity; i++)
            ExpandPool();
    }

    public T Get()
    {
        if (_objects.Count == 0)
            ExpandPool();

        T newObj = _objects.Dequeue();
        newObj.transform.rotation = Quaternion.identity;
        newObj.gameObject.SetActive(true);

        return newObj;
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }

    private void ExpandPool()
    {
        T obj = Object.Instantiate(_objectPrefab);
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }
}
