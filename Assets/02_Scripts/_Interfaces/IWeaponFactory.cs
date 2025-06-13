using UnityEngine;

public interface IWeaponFactory
{
    //WeaponBase Create(WeaponBaseSO data, Vector3 position, Quaternion rotation);
    WeaponBase Create(WeaponType type, Transform parent = null);

    /// <summary>
    /// Return weapon data ScriptableObject for the given type.
    /// </summary>
    WeaponBaseSO GetWeaponData(WeaponType type);

    void Release(WeaponBase unit);
}
