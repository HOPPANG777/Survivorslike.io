using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionUIController : SerializedMonoBehaviour
{
    [TabGroup("Weapon Choice UI")][SerializeField] private Image _icon;   // 무기 아이콘 표시용(필요 시)
    [TabGroup("Weapon Choice UI")][SerializeField] private TextMeshProUGUI _label;  // 무기 이름 표시용
    [TabGroup("Weapon Choice UI")][SerializeField] private TextMeshProUGUI _statLabel; // 스탯 차이 표시용 (deprecated)
    [TabGroup("Weapon Choice UI")][SerializeField] private Transform _statListParent; // 아이콘+수치 표시 부모
    [TabGroup("Weapon Choice UI")][SerializeField] private WeaponStatItemUIController _statItemPrefab; // 아이콘+수치 프리팹
    [TabGroup("Type, Name")][SerializeField] private Dictionary<string, Sprite> _statIcons; // 스탯 라벨별 아이콘

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
    public void SetData(WeaponType type, string weaponName, List<WeaponStatDiff> stats)
    {
        if (_label != null) _label.text = weaponName;

        if (_statListParent != null && _statItemPrefab != null)
        {
            foreach (Transform child in _statListParent) Destroy(child.gameObject);
            if (stats != null)
            {
                foreach (var s in stats)
                {
                    var item = Instantiate(_statItemPrefab, _statListParent);
                    Sprite icon = null;
                    if (_statIcons != null) _statIcons.TryGetValue(s.Label, out icon);
                    item.SetData(icon, s.Value);
                }
            }
        }
        else if (_statLabel != null && stats != null)
        {
            // fallback: 문자열로 표현
            var parts = new List<string>();
            foreach (var s in stats)
            {
                string sign = s.Value > 0 ? "+" : string.Empty;
                parts.Add($"{s.Label} {sign}{s.Value:0.#}");
            }
            _statLabel.text = string.Join(", ", parts);
        }
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
