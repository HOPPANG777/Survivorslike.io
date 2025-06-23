using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 무기 아이콘에 마우스 포인터가 올라가면 툴팁을 표시하는 컴포넌트
/// </summary>
public class WeaponImageHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private WeaponBase _weapon;
    private WeaponTooltipController _tooltip;

    /// <summary>
    /// 초기화 함수. 표시할 무기와 툴팁 컨트롤러를 지정합니다.
    /// </summary>
    public void Init(WeaponBase weapon, WeaponTooltipController tooltip)
    {
        _weapon = weapon;
        _tooltip = tooltip;
    }

    /// <summary>
    /// 마우스가 아이콘 위에 올라갔을 때 호출됩니다.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltip?.ShowTooltip(_weapon, eventData.position);
    }

    /// <summary>
    /// 마우스가 아이콘에서 벗어났을 때 호출됩니다.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip?.HideTooltip();
    }
}
