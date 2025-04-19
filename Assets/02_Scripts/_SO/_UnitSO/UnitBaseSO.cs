using UnityEngine;

//==============================================================================
// UnitBaseSO
//==============================================================================
// UnitBase Scriptable Object 기본적인 세팅을 빨리 하기 위한것
//	
//==============================================================================

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]

public class UnitBaseSO : ScriptableObject, ISetSOData<UnitBaseSO>
{
    [Header("UnitBaseSO")]
    [Tooltip("유닛 이름")][SerializeField] protected string _unitName;
    public string UnitName { get { return _unitName; } set { _unitName = value; } }

    [Tooltip("팀 타입")][SerializeField] private TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    [Tooltip("ISpeakable")][SerializeField] protected ISpeakable _unitSpeaker;
    public ISpeakable UnitSpeaker { get { return _unitSpeaker; } set { _unitSpeaker = value; } }

    virtual public void SetData(UnitBaseSO unitData)
    {
        this.UnitName = unitData.UnitName;

        this.TeamType = unitData.TeamType;

        this.UnitSpeaker = unitData.UnitSpeaker;
    }    
}
