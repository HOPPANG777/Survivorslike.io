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
    [Tooltip("Player Data")][SerializeField] private PlayerUnitSO _playerData;
    public PlayerUnitSO PlayerData { get { return _playerData; } set { _playerData = value; } }

    public PlayerableAttribute Attributes { get { return PlayerData.PlayerAttribute.Attributes; } set { PlayerData.PlayerAttribute.Attributes = value; } }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
