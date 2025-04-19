using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//==============================================================================
// PlayerUnitSO
//==============================================================================
// PlayerUnit Scriptable Object 기본적인 세팅을 빨리 하기 위한것
//	
//==============================================================================

[CreateAssetMenu(fileName = "PlayerData", menuName = "UnitData/PlayerData", order = 1)]

public class PlayerUnitSO : UnitBaseSO
{
    [Header("PlayerData")]
    [SerializeField] protected PlayerableAttribute _playerAttribute;
    public PlayerableAttribute PlayerAttribute { get { return _playerAttribute; } set { _playerAttribute = value; } }

    override public void SetData(UnitBaseSO _unitData)
    {
        base.SetData(_unitData);

        PlayerUnitSO _playerData = _unitData as PlayerUnitSO;

        this.PlayerAttribute = _playerData.PlayerAttribute;
    }
}
