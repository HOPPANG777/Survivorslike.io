using UnityEngine;

public interface IWeaponFactory
{
    //WeaponBase Create(WeaponBaseSO data, Vector3 position, Quaternion rotation);
    WeaponBase Create(WeaponType type, Transform parent = null);

    void Release(WeaponBase unit);
}
