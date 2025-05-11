using UnityEngine;

public interface IPoolable
{
    void OnReturnedToPool();
    void OnTakenFromPool();
}
