using UnityEngine;

//==============================================================================
// PlayerUnit
//==============================================================================
// PlayerUnit 스크립트
//	
//==============================================================================

public class PlayerUnit : UnitBase, IPlayerAttribute
{
    [Header("Player Unit")]
    private PlayerUnitSO _playerData;
    public PlayerUnitSO PlayerData { get { return _playerData; } set { _playerData = value; } }

    [Tooltip("Player Attribute")][SerializeField] private PlayerableAttribute _playerAttribute;
    public new PlayerableAttribute Attributes { get { return _playerAttribute; } set { _playerAttribute = value; } }

    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
    }

    override public void SetStatus()
    {
        base.SetStatus();
        _playerData = _unitBaseData as PlayerUnitSO;
        _playerAttribute = _playerData.PlayerAttribute;
    }
}
