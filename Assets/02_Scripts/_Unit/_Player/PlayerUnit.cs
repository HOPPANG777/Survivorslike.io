using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//==============================================================================
// PlayerUnit
//==============================================================================
// PlayerUnit 스크립트
//	
//==============================================================================

public class PlayerUnit : UnitBase, IPlayerAttribute, IWeaponHolder, IPlayerHolder
{
    [Header("Player Unit")]
    private PlayerUnitSO _playerData;
    public PlayerUnitSO PlayerData { get { return _playerData; } set { _playerData = value; } }

    [TabGroup("Player Unit")][Tooltip("Player Attribute")][SerializeField] private PlayerableAttribute _playerAttribute;
    public new PlayerableAttribute Attributes { get { return _playerAttribute; } set { _playerAttribute = value; } }

    [TabGroup("Player Unit")][Tooltip("Weapon Holder")][SerializeField] private List<WeaponBase> _equippedWeapons = new List<WeaponBase>();
    public List<WeaponBase> EquippedWeapons { get { return _equippedWeapons; } }

    [TabGroup("Player Unit")][Tooltip("AttackStatsProvider")][SerializeField] private AttackStatsProvider _attackStatsProvider;

    private const int MaxWeaponCount = 5;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public IReadOnlyList<WeaponBase> GetOwnedWeaponTypes()
    {
        return _equippedWeapons;
    }

    public void AddWeapon(WeaponType type)
    {
        // 1. 이미 소유한 무기라면 → 레벨만 올리고 끝
        for (int i = 0; i < _equippedWeapons.Count; i++)
        {
            WeaponBase weaponEquipped = _equippedWeapons[i];
            if (weaponEquipped.WeaponType == type)
            {
                weaponEquipped.LevelUp(); // 무기 레벨업
                var panel = AllManager.instance.UIManager.PlayerCurrentAttributesPanelController;
                if (panel != null) panel.UpdatePanel();
                return;
            }
        }

        // 새로운 무기 추가 전에 최대 보유 수 체크
        if (_equippedWeapons.Count >= MaxWeaponCount)
        {
            Debug.Log("Maximum weapon count reached");
            return;
        }

        this.gameObject.GetComponent<PlayerExpMagnetController>().SetStatus();
        
        // 2. 없다면 → 팩토리로 생성, 플레이어 자식으로 붙이고 리스트에 추가
        var weapon = AllManager.instance.WeaponFactory.Create(type, this.transform);
        if (weapon == null)
        {
            Debug.LogWarning($"WeaponFactory에서 {type} 무기 생성 실패");
            return;
        }

        _equippedWeapons.Add(weapon);
        {
            var panel = AllManager.instance.UIManager.PlayerCurrentAttributesPanelController;
            if (panel != null) panel.UpdatePanel();
        }
    }

    public override void SetStatus()
    {
        base.SetStatus();
        _playerData = _unitBaseData as PlayerUnitSO;
        _playerAttribute = _playerData.PlayerAttribute;
        _attackStatsProvider = this.gameObject.GetComponent<AttackStatsProvider>();
        _attackStatsProvider.SetStatus();
        _invincibleDuration = 0.1f; // 플레이어는 무적 시간 0.1초로 설정
        this.gameObject.GetComponent<PlayerDashController>().SetStatus();
        this.gameObject.GetComponent<PlayerGetExpController>().SetStatus();
        this.gameObject.GetComponent<PlayerLevelUpController>().SetStatus();
        this.gameObject.GetComponent<PlayerMouseDirectionController>().SetStatus();
        this.gameObject.GetComponent<PlayerMoveController>().SetStatus();
    }

    public override void Hitted(float damageValue)
    {
        base.Hitted(damageValue);
        var panel = AllManager.instance.UIManager.PlayerCurrentAttributesPanelController;
        if (panel != null) panel.UpdatePanel();
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }

    public override void Dead()
    {
        base.Dead();
        AllManager.instance.UIManager.GameOverPage();
        AllManager.instance.UnitFactory.Release(this.gameObject.GetComponent<PlayerUnit>());
    }
}
