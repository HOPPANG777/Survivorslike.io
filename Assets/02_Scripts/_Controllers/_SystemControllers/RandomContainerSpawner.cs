using Sirenix.OdinInspector;
using UnityEngine;

//==============================================================================
// RandomContainerSpawner
//==============================================================================
// 컨테이너 유닛을 일정 시간마다 랜덤 위치에 스폰하는 컨트롤러
// - ISpawnPositionProvider를 통해 위치 의존성을 제거
// - IUnitFactory를 사용하여 실제 유닛 생성
//==============================================================================
[DisallowMultipleComponent]
public class RandomContainerSpawner : SerializedMonoBehaviour, IContainerSpawner
{
    [TabGroup("Spawn Settings")]
    [Tooltip("스폰할 컨테이너 데이터")]
    [SerializeField] private UnitBaseSO _containerData;

    [TabGroup("Spawn Settings")]
    [Tooltip("스폰 간격(초)")]
    [SerializeField] private float _spawnInterval = 10f;

    [TabGroup("Spawn Settings")]
    [Tooltip("최대 스폰 수")]
    [SerializeField] private int _maxSpawnCount = 3;

    private float _timer;                   // 스폰 타이머
    private int _spawnedCount;              // 현재까지 스폰된 개수
    private IUnitFactory _unitFactory;      // 유닛 생성용 팩토리
    private ISpawnPositionProvider _provider; // 위치 제공자

    /// <summary>
    /// 스폰 시스템 초기화
    /// </summary>
    /// <param name="provider">스폰 위치 제공자</param>
    public void Init(ISpawnPositionProvider provider)
    {
        _unitFactory = AllManager.instance.UnitFactory;
        _provider = provider;
        _timer = 0f;
        _spawnedCount = 0;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        if (_unitFactory == null || _provider == null || _containerData == null) return;

        _timer += Time.deltaTime;
        if (_timer < _spawnInterval) return;
        if (_spawnedCount >= _maxSpawnCount) return;

        _timer -= _spawnInterval;
        Vector3 pos = _provider.GetPosition();
        UnitBase unit = _unitFactory.Create(_containerData, pos);
        if (unit != null)
        {
            _spawnedCount += 1;
        }
    }
}
