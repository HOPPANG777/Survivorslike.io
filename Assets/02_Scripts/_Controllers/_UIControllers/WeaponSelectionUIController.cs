using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionUIController : SerializedMonoBehaviour
{
    [TabGroup("Weapon Choice UI")][SerializeField] private Image _icon;   // 무기 아이콘 표시용(필요 시)
    [TabGroup("Weapon Choice UI")][SerializeField] private TextMeshProUGUI _label;  // 무기 이름 표시용

    private System.Action _onClick;

    [TabGroup("Type, Name")][SerializeField] private Dictionary<WeaponType, Sprite> _weaponLabel;

    public void Init(System.Action onClick)
    {
        _onClick = onClick;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() => _onClick());
    }

    /// <summary>
    /// 버튼의 표시 정보 설정
    /// </summary>
    public void SetData(WeaponType type)
    {
        if (_label != null) _label.text = type.ToString();
        // 아이콘을 사용한다면
        // 아이콘 딕셔너리에서 스프라이트 할당
        if (_icon != null && _weaponLabel != null && _weaponLabel.TryGetValue(type, out var sprite))
        {
            _icon.sprite = sprite;
            _icon.enabled = true; // 혹시 비활성화되어 있으면
        }
        else if (_icon != null)
        {
            _icon.enabled = false; // 해당 타입 스프라이트 없으면 감추기(선택)
        }
    }
}
