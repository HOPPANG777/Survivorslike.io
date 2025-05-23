using UnityEngine;

public interface IWeaponLevel
{
    int CurrentLevel { get; set; }
    int MaxLevel { get; }
    bool LevelUp();                // 레벨업 시도, 성공하면 true
    void ApplyLevelAttributes();   // 레벨 변경 후 속성 재적용
}
