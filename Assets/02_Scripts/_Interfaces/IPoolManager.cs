using UnityEngine;

public interface IPoolManager : IObjectPool
{
    GameObject Get(GameObject prefab);
}
