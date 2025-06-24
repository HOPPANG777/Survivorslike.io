using UnityEngine;

public interface IUIFactory
{
    GameObject Create(UIType type, Transform parent = null);
    void Release(GameObject uiObject);
    void ReleaseAll();
}

public enum UIType
{
    UI_NONE = 0,
    UI_UNIT_HEALTHBAR,
    // 플레이어 전용 체력 바
    UI_PLAYER_HEALTHBAR,
    UI_DAMAGE_TEXT,
}