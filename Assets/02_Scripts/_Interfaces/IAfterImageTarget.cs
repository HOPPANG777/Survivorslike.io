using UnityEngine;

/// <summary>
/// 잔상 효과 생성을 위해 필요한 정보를 제공하는 인터페이스
/// </summary>
public interface IAfterImageTarget
{
    /// <summary>잔상에 사용할 렌더러</summary>
    SpriteRenderer Renderer { get; }
    /// <summary>잔상 생성 주기</summary>
    float AfterImageInterval { get; }
    /// <summary>잔상 유지 시간</summary>
    float AfterImageDuration { get; }
}
