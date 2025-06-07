using System.Collections.Generic;
using UnityEngine;

public interface IPlayerHolder
{
    IReadOnlyList<WeaponType> GetOwnedWeaponTypes();

    void AddWeapon(WeaponType type);
}
