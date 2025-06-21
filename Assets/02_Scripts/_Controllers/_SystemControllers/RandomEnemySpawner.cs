using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomEnemySpawner : SerializedMonoBehaviour
{
    [TabGroup("Spawn Settings")][Tooltip("다양한 몬스터 스폰 설정 리스트")] [SerializeField] private List<SpawnEntry> _spawnEntries = new List<SpawnEntry>();
    [TabGroup("Spawn Settings")][Tooltip("보스 스폰 시간(초)")] [SerializeField] private float _bossSpawnTime = 60f;
    [TabGroup("Spawn Settings")][Tooltip("보스 데이터 SO")][SerializeField]private UnitBaseSO _bossData;
    [TabGroup("Spawn Settings")][Tooltip("보스 스폰 위치")][SerializeField]private Transform _bossSpawnPoint;

    private float _elapsedTime;
    private bool _bossSpawned;
    private IUnitFactory _factory;

    public void Init()
    {
        _factory = AllManager.instance.UnitFactory;
        _elapsedTime = 0f;
        _bossSpawned = false;

        // 구조체 리스트 초기화: 복사 후 필드 설정, 다시 대입
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

        // 보스 스폰 조건
        if (!_bossSpawned && _elapsedTime >= _bossSpawnTime)
        {
            ClearAllEnemies();
            SpawnBoss();
            _bossSpawned = true;
            return;
        }

        // 보스 이전에 일반 몬스터 스폰
        if (_bossSpawned) return;

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

            // 스폰 실행
            Vector3 pos = GetRandomOffCameraPosition();
            var enemy = _factory.Create(entry.EnemyData, pos);
            entry.SpawnedCount++;
            _spawnEntries[i] = entry;
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

    private void SpawnBoss()
    {
        Vector3 pos = _bossSpawnPoint != null ? _bossSpawnPoint.position : GetRandomOffCameraPosition();
        _factory.Create(_bossData, pos);
    }

    private void ClearAllEnemies()
    {
        AllManager.instance.UnitFactory.ReleaseAll();
    }

    // _offCameraMargin 필드도 유지
    [TabGroup("Spawn Settings")][SerializeField] private float _offCameraMargin = 1f;
}
