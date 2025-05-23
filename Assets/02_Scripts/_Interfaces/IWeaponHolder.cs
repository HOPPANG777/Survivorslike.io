using System.Collections.Generic;
using UnityEngine;

public interface IWeaponHolder
{
    List<WeaponBase> EquippedWeapons { get; }
}
