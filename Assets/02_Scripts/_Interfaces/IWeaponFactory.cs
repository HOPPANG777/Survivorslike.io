using UnityEngine;

public interface IWeaponFactory
{
    WeaponBase Create(WeaponBaseSO data, Vector3 position, Quaternion rotation);

    void Release(WeaponBase unit);
}
