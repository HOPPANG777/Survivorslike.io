using UnityEngine;

public interface IParticleFactory
{
    GameObject Create(ParticleType type, Vector3 position, Quaternion rotation, Transform parent = null);
    void Release(GameObject particle);
    void ReleaseAll();
}

public enum ParticleType
{
    PARTICLE_NONE = 0,
    PARTICLE_HIT
}
