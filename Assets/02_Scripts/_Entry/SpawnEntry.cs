using UnityEngine;

/// <summary>
/// 적 스폰 설정 엔트리 구조체
/// </summary>
[System.Serializable]
public struct SpawnEntry
{
    [Tooltip("스폰할 적 데이터 SO")] public UnitBaseSO EnemyData;
    [Tooltip("스폰 가능 시작 시간(초)")] public float StartTime;
    [Tooltip("스폰 가능 종료 시간(초)")] public float EndTime;
    [Tooltip("이 엔트리 내에서의 스폰 간격(초)")] public float Interval;
    [Tooltip("최대 스폰 개수")] public int MaxCount;
    [Tooltip("스폰 패턴")] public SpawnPatternType Pattern;
    [Tooltip("한 번에 스폰할 개수")] public int SpawnAmount;

    // 내부 사용
    [HideInInspector] public float Timer;
    [HideInInspector] public int SpawnedCount;
}
