using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

/// <summary>
/// 플레이어의 현재 무기 현황과 능력치를 표시하는 패널 컨트롤러
/// </summary>
public class PlayerCurrentAttributesPanelController : MonoBehaviour, ISetStatus
{
    [TabGroup("UI")][Tooltip("플레이어가 보유한 무기 정보를 표시할 텍스트")]
    [SerializeField] private TextMeshProUGUI _weaponInfoText;

    [TabGroup("UI")][Tooltip("플레이어의 주요 스탯을 표시할 텍스트")]
    [SerializeField] private TextMeshProUGUI _statInfoText;

    private PlayerUnit _player;                             // 플레이어 오브젝트
    private AttackStatsProvider _statsProvider;             // 공격 스탯 계산기
    private IUnitAttribute<UnitAttribute> _unitAttributes;  // 기본 유닛 스탯 접근용

    /// <summary>
    /// 패널에서 사용할 모든 참조를 초기화합니다.
    /// </summary>
    public void SetStatus()
    {
        _player = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();
        _statsProvider = _player.GetComponent<AttackStatsProvider>();
        _unitAttributes = _player.GetComponent<IUnitAttribute<UnitAttribute>>();
        UpdatePanel();
    }

    /// <summary>
    /// 현재 플레이어의 무기와 스탯 정보를 갱신합니다.
    /// </summary>
    public void UpdatePanel()
    {
        if (_player == null) return;

        // 무기 정보 문자열 생성
        string weaponText = string.Empty;
        var weapons = _player.EquippedWeapons;
        for (int i = 0; i < weapons.Count; i++)
        {
            WeaponBase weapon = weapons[i];
            weaponText += $"{weapon.WeaponName} Lv.{weapon.CurrentLevel + 1}";
            if (i < weapons.Count - 1) weaponText += "\n";
        }
        if (_weaponInfoText != null) _weaponInfoText.text = weaponText;

        // 스탯 정보 문자열 생성
        if (_statInfoText != null && _statsProvider != null && _unitAttributes != null)
        {
            var attr = _unitAttributes.Attributes;
            string statText =
                $"HP {attr.UnitCurrentHealth:0}/{attr.UnitMaxHealth:0}\n" +
                $"공격력 {_statsProvider.AttackPower:0.##}\n" +
                $"공격 속도 {_statsProvider.AttackSpeed:0.##}\n" +
                $"투사체 수 {_statsProvider.ProjectileCount}\n" +
                $"사거리 {_statsProvider.AttackRange:0.##}\n" +
                $"쿨타임 {_statsProvider.AttackCooldown:0.##}";
            _statInfoText.text = statText;
        }
    }
}

