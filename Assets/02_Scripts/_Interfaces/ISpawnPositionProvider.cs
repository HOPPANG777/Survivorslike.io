using UnityEngine;

/// <summary>
/// 스폰 위치를 제공하는 인터페이스
/// </summary>
public interface ISpawnPositionProvider
{
    /// <summary>
    /// 생성될 위치를 반환
    /// </summary>
    Vector3 GetPosition();
}
