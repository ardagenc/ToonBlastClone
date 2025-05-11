using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            AddNewObject();
        }
    }

    private T AddNewObject()
    {
        T obj = GameObject.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public T Get()
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : AddNewObject();
        obj.gameObject.SetActive(true);

        if (obj is IPoolable p) p.OnTakenFromPool();
        return obj;
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);

        if (obj is IPoolable p) p.OnReturnedToPool();
        pool.Enqueue(obj);
    }
}
