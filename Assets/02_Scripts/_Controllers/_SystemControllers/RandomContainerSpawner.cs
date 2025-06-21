using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

//==============================================================================
// RandomContainerSpawner
//==============================================================================
// 컨테이너 유닛을 일정 시간마다 랜덤 위치에 스폰하는 컨트롤러
// - RandomEnemySpawner 로직을 활용하여 구현 단순화
// - IUnitFactory를 사용하여 실제 유닛 생성
//==============================================================================
[DisallowMultipleComponent]
public class RandomContainerSpawner : SerializedMonoBehaviour
{
    [TabGroup("Spawn Settings")][Tooltip("스폰 설정 목록")]
    [SerializeField] private List<SpawnEntry> _spawnEntries = new List<SpawnEntry>();

    private float _elapsedTime;
    private IUnitFactory _factory;

    public void Init()
    {
        _factory = AllManager.instance.UnitFactory;
        _elapsedTime = 0f;

        // 스폰 관련 값 초기화
        for (int i = 0; i < _spawnEntries.Count; i++)
        {
            SpawnEntry entry = _spawnEntries[i];
            entry.Timer = 0f;
            entry.SpawnedCount = 0;
            _spawnEntries[i] = entry;
        }
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        _elapsedTime += Time.deltaTime;

        for (int i = 0; i < _spawnEntries.Count; i++)
        {
            var entry = _spawnEntries[i];
            if (_elapsedTime < entry.StartTime || _elapsedTime > entry.EndTime) continue;
            if (entry.SpawnedCount >= entry.MaxCount) continue;

            entry.Timer += Time.deltaTime;
            if (entry.Timer < entry.Interval)
            {
                _spawnEntries[i] = entry;
                continue;
            }

            entry.Timer -= entry.Interval;
            _spawnEntries[i] = entry;

            Vector3 pos = GetRandomOffCameraPosition();
            var container = _factory.Create(entry.EnemyData, pos);
            if (container != null)
            {
                entry.SpawnedCount++;
                _spawnEntries[i] = entry;
            }
        }
    }

    private Vector3 GetRandomOffCameraPosition()
    {
        Camera cam = Camera.main;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 center = cam.transform.position;
        float m = _offCameraMargin;
        switch (Random.Range(0, 4))
        {
            case 0: return new Vector3(center.x - halfW - m, Random.Range(center.y - halfH, center.y + halfH), 0f);
            case 1: return new Vector3(center.x + halfW + m, Random.Range(center.y - halfH, center.y + halfH), 0f);
            case 2: return new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y + halfH + m, 0f);
            default: return new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y - halfH - m, 0f);
        }
    }

    [TabGroup("Spawn Settings")][SerializeField] private float _offCameraMargin = 1f;
}
