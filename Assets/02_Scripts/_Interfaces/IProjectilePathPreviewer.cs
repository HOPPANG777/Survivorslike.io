using UnityEngine;

/// <summary>
/// 투사체 경로 미리보기를 위한 인터페이스.
/// </summary>
public interface IProjectilePathPreviewer
{
    // 미리보기를 제어할 오브젝트 지정
    void Initialize(Transform owner);
    // 방향과 길이를 갱신
    void UpdatePath(Vector3 direction, float length);
    // 미리보기 표시
    void Show();
    // 미리보기 숨김
    void Hide();
    // 발사까지의 진행도 설정 (0~1)
    void SetChargeProgress(float ratio);
}
