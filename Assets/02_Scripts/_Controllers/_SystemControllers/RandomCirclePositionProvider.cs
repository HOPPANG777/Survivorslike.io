using UnityEngine;

/// <summary>
/// 특정 중심을 기준으로 반경 내 임의의 위치를 반환하는 클래스
/// </summary>
public class RandomCirclePositionProvider : MonoBehaviour, ISpawnPositionProvider
{
    [SerializeField] private Transform _center; // 기준이 되는 중심 오브젝트
    [SerializeField] private float _radius = 5f; // 위치를 생성할 반경

    /// <summary>
    /// 중심과 반경을 설정
    /// </summary>
    /// <param name="center">중심 오브젝트</param>
    /// <param name="radius">반경</param>
    public void Setup(Transform center, float radius)
    {
        _center = center;
        _radius = radius;
    }

    /// <summary>
    /// 반경 내 임의의 위치를 반환
    /// </summary>
    public Vector3 GetPosition()
    {
        Vector2 random = Random.insideUnitCircle * _radius;
        Vector3 basePos = _center != null ? _center.position : Vector3.zero;
        return basePos + new Vector3(random.x, random.y, 0f);
    }
}
