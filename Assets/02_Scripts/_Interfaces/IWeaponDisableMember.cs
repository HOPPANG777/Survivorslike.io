using UnityEngine;

public interface IWeaponDisableMember
{
    WeaponDisableType WeaponDisableType { get; set; }
}

public enum WeaponDisableType
{
    WEAPONDISABLETYPE_NONE = 0,
    WEAPONDISABLETYPE_COUNT, // 무기 관통이나 반사를 통해서 카운트가 감소되서 0이 되면 사라지는 무기
    WEAPONDISABLETYPE_TIME   // 무기 사용시간이 지나면 사라지는 무기
}