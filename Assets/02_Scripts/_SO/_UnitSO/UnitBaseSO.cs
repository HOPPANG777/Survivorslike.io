using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//==============================================================================
// UnitBaseSO
//==============================================================================
// UnitBase Scriptable Object 기본적인 세팅을 빨리 하기 위한것
//	
//==============================================================================

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData/UnitData", order = 0)]

public class UnitBaseSO : ScriptableObject, ISetSOData<UnitBaseSO>
{
    [TabGroup("UnitBaseSO")][SerializeField] protected UnitAttribute _unitAttribute;
    public UnitAttribute UnitAttribute { get { return _unitAttribute; } set { _unitAttribute = value; } }

    [TabGroup("UnitBaseSO")][Tooltip("유닛 스프라이트")][SerializeField] protected Sprite _unitSprite;
    public Sprite UnitSprite { get { return _unitSprite; } set { _unitSprite = value; } }

    [TabGroup("UnitBaseSO")][Tooltip("유닛 이름")][SerializeField] protected string _unitName;
    public string UnitName { get { return _unitName; } set { _unitName = value; } }

    [TabGroup("UnitBaseSO")][Tooltip("팀 타입")][SerializeField] protected TeamType _teamType;
    public TeamType TeamType { get { return _teamType; } set { _teamType = value; } }

    // 사용할 사운드 이름
    [TabGroup("Sound")][Tooltip("사운드 이름")][SerializeField] protected string _soundName;
    public string SoundName { get { return _soundName; } }

    // 사용할 사운드 타입
    [TabGroup("Sound")][Tooltip("사운드 타입")][SerializeField] protected SoundType _soundType;
    public SoundType SoundType { get { return _soundType; } }

    // 사용할 오디오 클립
    [TabGroup("Sound")][Tooltip("오디오 클립")][SerializeField] protected AudioClip _soundClip;
    public AudioClip SoundClip { get { return _soundClip; } }

    public virtual void SetData(UnitBaseSO unitData)
    {
        this.UnitAttribute = unitData.UnitAttribute;

        this.UnitSprite = unitData.UnitSprite;

        this.UnitName = unitData.UnitName;

        this.TeamType = unitData.TeamType;

        this._soundName = unitData._soundName;

        this._soundType = unitData._soundType;

        this._soundClip = unitData._soundClip;
    }
}
