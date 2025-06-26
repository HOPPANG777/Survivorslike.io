using UnityEngine;

/// <summary>
/// 도트 스폰 적의 이동 방향을 지정하기 위한 인터페이스
/// </summary>
public interface IDotMove
{
    /// <summary>
    /// 이동할 방향 설정
    /// </summary>
    /// <param name="direction">방향 벡터</param>
    void SetDirection(Vector3 direction);
}
