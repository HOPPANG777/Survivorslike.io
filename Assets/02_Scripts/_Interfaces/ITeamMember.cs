using UnityEngine;

//==============================================================================
// ITeamMember
//==============================================================================
// 플레이어블, 적, 오브젝트, 우호적 객체등을 구분하기 위한 인터페이스
// 이것또한 의존성 관리를 위해 분리해놓았다. 
//
//==============================================================================
public interface ITeamMember
{
    TeamType TeamType { get; set; }
}

public enum TeamType
{ 
    TEAMTYPE_NONE = 0,
    TEAMTYPE_ALLY,
    TEAMTYPE_ENEMY,
    TEAMTYPE_NEUTRAL
}
