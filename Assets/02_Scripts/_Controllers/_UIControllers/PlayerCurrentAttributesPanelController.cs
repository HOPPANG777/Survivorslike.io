using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 플레이어의 현재 무기 현황과 능력치를 표시하는 패널 컨트롤러
/// </summary>
public class PlayerCurrentAttributesPanelController : MonoBehaviour, ISetStatus
{
    [TabGroup("UI")][Tooltip("무기 아이콘을 표시할 이미지들 (최대 5개)")][SerializeField] private List<Image> _weaponImages = new List<Image>();
    [TabGroup("UI")][Tooltip("악세서리 아이콘을 표시할 이미지들 (최대 5개)")][SerializeField] private List<Image> _accImages = new List<Image>();
    [TabGroup("UI")][Tooltip("무기 레벨을 표시할 텍스트들 (최대 5개)")][SerializeField] private List<TextMeshProUGUI> _weaponLevelTexts = new List<TextMeshProUGUI>();
    [TabGroup("UI")][Tooltip("무기 정보를 표시할 툴팁 컨트롤러")][SerializeField] private WeaponTooltipController _weaponTooltipController;

    [TabGroup("UI")][Tooltip("플레이어 체력")][SerializeField]
    private TextMeshProUGUI _healthText;
    [TabGroup("UI")][Tooltip("방어력")][SerializeField]
    private TextMeshProUGUI _defenseText;
    [TabGroup("UI")][Tooltip("이동 속도")][SerializeField]
    private TextMeshProUGUI _moveSpeedText;
    [TabGroup("UI")][Tooltip("행운")][SerializeField]
    private TextMeshProUGUI _luckText;
    [TabGroup("UI")][Tooltip("경험치 획득 범위")][SerializeField]
    private TextMeshProUGUI _getExpRangeText;
    [TabGroup("UI")][Tooltip("기본 공격력")][SerializeField]
    private TextMeshProUGUI _baseDamageText;
    [TabGroup("UI")][Tooltip("공격 속도")][SerializeField]
    private TextMeshProUGUI _attackSpeedText;
    [TabGroup("UI")][Tooltip("사거리")][SerializeField]
    private TextMeshProUGUI _attackRangeText;
    [TabGroup("UI")][Tooltip("쿨타임")][SerializeField]
    private TextMeshProUGUI _attackCooldownText;
    [TabGroup("UI")][Tooltip("공격 지속 시간")][SerializeField]
    private TextMeshProUGUI _attackDurationText;
    [TabGroup("UI")][Tooltip("투사체 수")][SerializeField]
    private TextMeshProUGUI _projectileNumberText;
    [TabGroup("UI")][Tooltip("넉백 힘")][SerializeField]
    private TextMeshProUGUI _knockbackForceText;
    [TabGroup("UI")][Tooltip("치명타 데미지")][SerializeField]
    private TextMeshProUGUI _criticalDamageText;
    [TabGroup("UI")][Tooltip("치명타 확률")][SerializeField]
    private TextMeshProUGUI _criticalChanceText;

    private UnitBase _player;                             // 플레이어 오브젝트 
    private AttackStatsProvider _statsProvider;             // 공격 스탯 계산기
    private IUnitAttribute<UnitAttribute> _unitAttributes;  // 플레이어 스탯 접근용
    private IUnitAttribute<PlayerableAttribute> _playerAttributes;  // 플레이어 스탯 접근용

    /// <summary>
    /// 패널에서 사용할 모든 참조를 초기화합니다.
    /// </summary>
    public void SetStatus()
    {
        _player = AllManager.instance.GamemodeManager.Player.GetComponent<PlayerUnit>();
        _statsProvider = _player.GetComponent<AttackStatsProvider>();
        _unitAttributes = _player.GetComponent<IUnitAttribute<UnitAttribute>>();
        _playerAttributes = _player.GetComponent<IUnitAttribute<PlayerableAttribute>>();
        _weaponTooltipController?.HideTooltip();
        UpdatePanel();
    }

    /// <summary>
    /// 현재 플레이어의 무기와 스탯 정보를 갱신합니다.
    /// </summary>
    public void UpdatePanel()
    {
        if (_player == null) return;

        PlayerUnit playerUnit = _player as PlayerUnit;

        var weapons = playerUnit.EquippedWeapons;
        if (_weaponImages != null)
        {
            for (int i = 0; i < _weaponImages.Count; i++)
            {
                if (i < weapons.Count)
                {
                    var weapon = weapons[i];
                    var img = _weaponImages[i];
                    if (img != null && weapon.WeaponBaseData != null)
                    {
                        img.sprite = weapon.WeaponBaseData.WeaponSprite;
                        img.enabled = img.sprite != null;

                        // 마우스 오버 시 정보를 보여주도록 핸들러 설정
                        var hover = img.GetComponent<WeaponImageHoverHandler>();
                        if (hover == null) hover = img.gameObject.AddComponent<WeaponImageHoverHandler>();
                        hover.Init(weapon, _weaponTooltipController);
                    }
                    if (_weaponLevelTexts != null && i < _weaponLevelTexts.Count)
                    {
                        var lvlTxt = _weaponLevelTexts[i];
                        if (lvlTxt != null)
                        {
                            lvlTxt.text = $"Lv.{weapon.CurrentLevel + 1}";
                        }
                    }
                }
                else if (_weaponImages[i] != null)
                {
                    _weaponImages[i].sprite = null;
                    _weaponImages[i].enabled = false;
                    var hover = _weaponImages[i].GetComponent<WeaponImageHoverHandler>();
                    if (hover != null) Destroy(hover);
                    if (_weaponLevelTexts != null && i < _weaponLevelTexts.Count && _weaponLevelTexts[i] != null)
                    {
                        _weaponLevelTexts[i].text = string.Empty;
                    }
                }
            }
        }

        if (_statsProvider != null && _unitAttributes != null)
        {
            var attr = _unitAttributes.Attributes;
            var playerAttr = playerUnit.Attributes;
            _healthText?.SetText($"{attr.UnitHealth:0}/{attr.UnitMaxHealth:0}");
            _defenseText?.SetText($"{attr.UnitDefense:0.##}");
            _moveSpeedText?.SetText($"{attr.UnitMoveSpeed:0.##}");
            _luckText?.SetText($"{playerAttr.UnitLuck:0.##}");
            _getExpRangeText?.SetText($"{playerAttr.PlayerGetExpRange:0.##}");
            _baseDamageText?.SetText($"{_statsProvider.AttackPower:0.##}");
            _attackSpeedText?.SetText($"{_statsProvider.AttackSpeed:0.##}");
            _attackRangeText?.SetText($"{_statsProvider.AttackRange:0.##}");
            _attackCooldownText?.SetText($"{_statsProvider.AttackCooldown:0.##}");
            _attackDurationText?.SetText($"{_statsProvider.AttackDuration:0.##}");
            _projectileNumberText?.SetText($"{_statsProvider.ProjectileCount}");
            _knockbackForceText?.SetText($"{_statsProvider.KnockbackForce:0.##}");
            _criticalDamageText?.SetText($"{attr.UnitCriticalDamage:0.##}");
            _criticalChanceText?.SetText($"{attr.UnitCritChance:0.##}");
        }
    }
}

