using System.Collections.Generic;
using UnityEngine;

public interface IPlayerHolder
{
    /// <summary>
    /// 플레이어가 소유 중인 모든 무기
    /// </summary>
    IReadOnlyList<WeaponBase> GetOwnedWeaponTypes();

    void AddWeapon(WeaponType type);
}
