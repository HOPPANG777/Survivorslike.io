using UnityEngine;

public interface IWeaponMember
{
    WeaponType WeaponType { get; set; }
}

[System.Serializable]
public enum WeaponType
{ 
    WEAPON_NONE = 0,
    WEAPON_SNIPER,  // 적 관통하는 저격총
    WEAPON_PISTOL,  // 일반 권총
    WEAPON_GATLING, // 탄막 흩뿌리기
    WEAPON_MISSILE, // 유도탄
    WEAPON_AEGIS,   // 뱀서의 성경.
    WEAPON_BOMB,    // 뱀서의 도끼
    WEAPON_FLAME,   // 화염방사기
    WEAPON_WIPE,    // 뱀서의 채찍
    WEAPON_SHOVEL,  // 야전삽 = 휘둘러서 공격
    WEAPON_DRONE,   // 드론 = 뱀서의 비둘기
    WEAPON_LASER    // 레이저 = 뱀서의 룬트레이서
}
