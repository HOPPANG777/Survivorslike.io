using UnityEngine;

public interface IProjectileFactory
{
    ProjectileBase Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation);

    void Release(ProjectileBase projectile);
}
