using UnityEngine;

public interface IProjectileFactory
{
    ProjectileBase Create(ProjectileBaseSO data, Vector3 position, Quaternion rotation);

    ProjectileBaseSO GetProjectileData(WeaponType type);
    void Release(ProjectileBase projectile);
}
