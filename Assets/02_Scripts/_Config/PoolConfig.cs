using UnityEngine;

[System.Serializable]
public class PoolConfig
{
    [Tooltip("풀링할 Prefab")]
    public GameObject Prefab;

    [Tooltip("초기 인스턴스 개수")]
    public int InitialSize = 5;
}
