using UnityEngine;

public interface IObjectPool
{
    GameObject Get();

    void Return(GameObject instance);
}
