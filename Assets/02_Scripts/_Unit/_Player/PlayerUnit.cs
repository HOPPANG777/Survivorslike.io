using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//==============================================================================
// PlayerUnit
//==============================================================================
// PlayerUnit 스크립트
//	
//==============================================================================

public class PlayerUnit : UnitBase, IPlayerAttribute, IWeaponHolder
{
    [Header("Player Unit")]
    private PlayerUnitSO _playerData;
    public PlayerUnitSO PlayerData { get { return _playerData; } set { _playerData = value; } }

    [TabGroup("Player Unit")][Tooltip("Player Attribute")][SerializeField] private PlayerableAttribute _playerAttribute;
    public new PlayerableAttribute Attributes { get { return _playerAttribute; } set { _playerAttribute = value; } }

    [TabGroup("Player Unit")][Tooltip("Weapon Holder")][SerializeField] private List<WeaponBase> _equippedWeapons = new List<WeaponBase>();
    public List<WeaponBase> EquippedWeapons { get { return _equippedWeapons; } }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetStatus()
    {
        base.SetStatus();
        _playerData = _unitBaseData as PlayerUnitSO;
        _playerAttribute = _playerData.PlayerAttribute;
    }

    public override void Hitted(float damageValue)
    {
        base.Hitted(damageValue);
        AllManager.instance.UIManager.PlayerCurrentHealthBarController.UpdatePlayerHealthBarImage(base.Attributes.UnitCurrentHealth / base.Attributes.UnitMaxHealth);
    }

    public override void CheckDead()
    {
        base.CheckDead();
    }

    public override void Dead()
    {
        base.Dead();
        AllManager.instance.UIManager.GameOverPage();
    }
}
