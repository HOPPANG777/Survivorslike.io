using UnityEngine;

/// <summary>
/// 투사체 경로 미리보기를 위한 인터페이스.
/// </summary>

public interface IProjectilePathPreviewer
{
    // 미리보기 대상 설정
    void Initialize(Transform owner);
    // 방향과 길이를 받아 경로를 갱신
    void UpdatePath(Vector3 direction, float length);
    // 미리보기를 화면에 표시
    void Show();
    // 미리보기를 숨김
    void Hide();
}


/*
    일단 무기들이 구분이 안된다는 친구들의 피드백을 반영하여
    되는대로 무기들에게 효과를 추가해주는 중이다.

 
*/