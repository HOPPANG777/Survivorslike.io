using UnityEngine;

/// <summary>
/// 체력 바 UI가 구현해야 할 기본 동작을 정의하는 인터페이스
/// </summary>
public interface IHealthBarUI
{
    /// <summary>
    /// 위치 제공자와 속성 제공자를 이용해 체력 바를 초기화한다
    /// </summary>
    /// <param name="positionProvider">월드 위치를 제공하는 객체</param>
    /// <param name="attributeProvider">체력 정보를 제공하는 객체</param>
    /// <param name="healthBarColor">체력 바 색상</param>
    void Bind(IPositionProvider positionProvider, IUnitAttribute<UnitAttribute> attributeProvider, Color healthBarColor);
}
