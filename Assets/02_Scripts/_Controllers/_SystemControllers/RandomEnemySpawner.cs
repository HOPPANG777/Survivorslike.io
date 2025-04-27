using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomEnemySpawner : SerializedMonoBehaviour
{
    [TabGroup("Spawn Settings")][Tooltip("스폰할 적 데이터 SO")][SerializeField] private UnitBaseSO _enemyData;
    [TabGroup("Spawn Settings")][Tooltip("스폰 지점(빈 값 시 카메라 밖 랜덤)")][SerializeField] private Transform[] _spawnPoints;
    [TabGroup("Spawn Settings")][Tooltip("카메라 밖으로부터 떨어진 거리")][SerializeField] private float _offCameraMargin = 1f;
    [TabGroup("Spawn Settings")][Tooltip("스폰 간격 (초)")][SerializeField] private float _spawnInterval = 5f;
    [TabGroup("Spawn Settings")][Tooltip("최대 활성 적 개수")][SerializeField] private int _maxEnemies = 10;

    private float _timer;
    private readonly List<UnitBase> _activeEnemies = new List<UnitBase>();
    private IUnitFactory _factory;

    private void Start()
    {
        _factory = UnitFactory.Instance;
    }

    private void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        _timer += Time.deltaTime;
        if (_timer < _spawnInterval) return;

        _timer -= _spawnInterval;
        if (_activeEnemies.Count >= _maxEnemies) return;

        SpawnOne();
    }

    private void SpawnOne()
    {
        Vector3 spawnPos;

        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            // 지정된 지점 중 랜덤 선택
            int idx = Random.Range(0, _spawnPoints.Length);
            spawnPos = _spawnPoints[idx].position;
        }
        else
        {
            // 카메라 바깥 랜덤 스폰
            Camera cam = Camera.main;
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            Vector3 center = cam.transform.position;
            float m = _offCameraMargin;

            switch (Random.Range(0, 4))
            {
                case 0: // 왼쪽
                    spawnPos = new Vector3(center.x - halfW - m, Random.Range(center.y - halfH, center.y + halfH), 0f);
                    break;
                case 1: // 오른쪽
                    spawnPos = new Vector3(center.x + halfW + m, Random.Range(center.y - halfH, center.y + halfH), 0f);
                    break;
                case 2: // 위
                    spawnPos = new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y + halfH + m, 0f);
                    break;
                default: // 아래
                    spawnPos = new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y - halfH - m, 0f);
                    break;
            }
        }

        var enemy = _factory.Create(_enemyData, spawnPos);
        _activeEnemies.Add(enemy);
    }

    private void OnEnemyDeath(UnitBase enemy)
    {
        _factory.Release(enemy);
        _activeEnemies.Remove(enemy);
    }

    /// <summary>
    /// 활성화된 모든 적 제거 및 타이머 초기화
    /// </summary>
    public void ClearAll()
    {
        foreach (var e in _activeEnemies)
        {
            _factory.Release(e);
        }
        _activeEnemies.Clear();
        _timer = 0f;
    }
}

