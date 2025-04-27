using UnityEngine;

public interface IProjectileFactory
{
    GameObject Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation);
    void Release(GameObject projectile);
}
