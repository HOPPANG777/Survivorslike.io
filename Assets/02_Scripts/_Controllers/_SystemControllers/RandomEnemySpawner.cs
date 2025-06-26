using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomEnemySpawner : SerializedMonoBehaviour
{
    [TabGroup("Spawn Settings")][Tooltip("다양한 몬스터 스폰 설정 리스트")][SerializeField]private List<SpawnEntry> _spawnEntries = new List<SpawnEntry>();
    [TabGroup("Spawn Settings")][Tooltip("보스 스폰 시간(초)")][SerializeField]private float _bossSpawnTime = 60f;
    [TabGroup("Spawn Settings")][Tooltip("보스 데이터 SO")][SerializeField]private UnitBaseSO _bossData;
    [TabGroup("Spawn Settings")][Tooltip("보스 스폰 위치")][SerializeField]private Transform _bossSpawnPoint;
    [TabGroup("Spawn Settings")][SerializeField] private float _offCameraMargin = 1f;

    private float _elapsedTime;
    private bool _bossSpawned;
    private IUnitFactory _factory;
    private IPositionProvider _playerPosition;

    public void Init()
    {
        _factory = AllManager.instance.UnitFactory;
        var player = AllManager.instance.GamemodeManager.Player;
        if (player != null && player.TryGetComponent<IPositionProvider>(out var provider))
        {
            _playerPosition = provider;
        }

        _elapsedTime = 0f;
        _bossSpawned = false;

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

        if (!_bossSpawned && _elapsedTime >= _bossSpawnTime)
        {
            ClearAllEnemies();
            SpawnBoss();
            _bossSpawned = true;
            return;
        }

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

            int spawnAmount = Mathf.Max(1, entry.SpawnAmount);
            spawnAmount = Mathf.Min(spawnAmount, entry.MaxCount - entry.SpawnedCount);

            SpawnByPattern(entry, spawnAmount);
            entry.SpawnedCount += spawnAmount;
            _spawnEntries[i] = entry;
        }
    }

    // 패턴에 따라 적 스폰
    private void SpawnByPattern(SpawnEntry entry, int count)
    {
        switch (entry.Pattern)
        {
            case SpawnPatternType.SPAWNPATTERN_DOT:
                SpawnDot(entry, count);
                break;
            case SpawnPatternType.SPAWNPATTERN_SURROUND:
                SpawnSurround(entry, count);
                break;
            case SpawnPatternType.SPAWNPATTERN_BOSS:
                SpawnBossFromEntry(entry);
                break;
            default:
                for (int i = 0; i < count; i++)
                {
                    SpawnRandom(entry);
                }
                break;
        }
    }

    // 무작위 위치에 스폰
    private void SpawnRandom(SpawnEntry entry)
    {
        Vector3 pos = GetRandomOffCameraPosition();
        _factory.Create(entry.EnemyData, pos);
    }

    // 일정 반경 내 임의의 위치에 점 형태로 스폰
    private void SpawnDot(SpawnEntry entry, int count)
    {
        Vector3 basePosition = GetRandomOutsideCameraPosition();

        // 플레이어 방향 계산
        Vector3 dir = Vector3.down;
        if (_playerPosition != null)
        {
            dir = (_playerPosition.Position - basePosition).normalized;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
            Vector3 spawnPosition = basePosition + offset;
            var unit = _factory.Create(entry.EnemyData, spawnPosition);
            if (unit == null) continue;

            if (unit.TryGetComponent<IDotMove>(out var mover))
            {
                var comp = unit.gameObject.GetComponent<DotEnemyMoveController>();
                if (comp is ISetStatus setter) setter.SetStatus();
                mover = comp;
            }

            mover.SetDirection(dir);
        }
    }

    // 플레이어를 둘러싸도록 스폰
    private void SpawnSurround(SpawnEntry entry, int count)
    {
        if (_playerPosition == null) return;
        Vector3 playerPos = _playerPosition.Position;
        float radiusStep = 360.0f / count;
        float angle = 0.0f;
        float radius = 18.0f;
        
        for (int i = 0; i < count; i++)
        {
            float enemyPosX = playerPos.x + Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float enemyPosY = playerPos.y + Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            Vector3 spawnPosition = new Vector3(enemyPosX, enemyPosY, playerPos.z);
            _factory.Create(entry.EnemyData, spawnPosition);
            angle += radiusStep;
        }
    }

    // 보스 패턴 스폰
    private void SpawnBossFromEntry(SpawnEntry entry)
    {
        Vector3 pos = GetCameraTopPosition();
        _factory.Create(entry.EnemyData, pos);
    }

    // 무작위 위치 계산
    private Vector3 GetRandomOffCameraPosition()
    {
        return GetRandomOutsideCameraPosition();
    }

    // 화면 밖 무작위 위치 계산
    private Vector3 GetRandomOutsideCameraPosition(float additionalMargin = 0f)
    {
        Camera cam = Camera.main;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 center = cam.transform.position;
        float m = _offCameraMargin + additionalMargin;
        switch (Random.Range(0, 4))
        {
            case 0: return new Vector3(center.x - halfW - m, Random.Range(center.y - halfH, center.y + halfH), 0f);
            case 1: return new Vector3(center.x + halfW + m, Random.Range(center.y - halfH, center.y + halfH), 0f);
            case 2: return new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y + halfH + m, 0f);
            default: return new Vector3(Random.Range(center.x - halfW, center.x + halfW), center.y - halfH - m, 0f);
        }
    }

    // 화면 내부 무작위 위치 계산
    private Vector3 GetRandomInsideCameraPosition()
    {
        Camera cam = Camera.main;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 center = cam.transform.position;
        return new Vector3
            (
                Random.Range
                (
                    center.x - halfW, 
                    center.x + halfW
                ),
                Random.Range
                (
                    center.y - halfH, 
                    center.y + halfH
                ),
                0f
            );
    }

    // 카메라 위쪽 위치 계산
    private Vector3 GetCameraTopPosition()
    {
        Camera cam = Camera.main;
        float halfH = cam.orthographicSize;
        Vector3 center = cam.transform.position;
        float m = _offCameraMargin;
        float x = _playerPosition != null ? _playerPosition.Position.x : center.x;
        return new Vector3(x, center.y + halfH + m, 0f);
    }

    private void SpawnBoss()
    {
        Vector3 pos = _bossSpawnPoint != null ? _bossSpawnPoint.position : GetCameraTopPosition();
        _factory.Create(_bossData, pos);
        // 보스가 등장하면 카메라 시야를 넓힌다
        AllManager.instance.GamemodeManager.CameraController.SetOrthographicSize(15f);
    }

    private void ClearAllEnemies()
    {
        AllManager.instance.UnitFactory.ReleaseAllEnemies();
    }

}
