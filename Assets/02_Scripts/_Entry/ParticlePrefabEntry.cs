using UnityEngine;

[System.Serializable]
public struct ParticlePrefabEntry
{
    [Tooltip("파티클 종류")]
    public ParticleType Type;

    [Tooltip("사용할 프리팹")]
    public GameObject Prefab;
}
