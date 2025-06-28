using UnityEngine;

//==============================================================================
// DroneWeaponProjectile
//==============================================================================
// DroneWeapon에서 발사되는 투사체. 베지어 곡선을 따라 이동하며
// 경로의 끝 지점에 도달하면 사라진다.
//==============================================================================

public class DroneWeaponProjectile : ProjectileBase
{
    // 베지어 곡선 제어점 네 개
    private readonly Vector3[] _points = new Vector3[4];

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;

    // 이동에 걸리는 총 시간
    private float _travelTime;
    // 경과 시간
    private float _timer;

    //--------------------------------------------------------------------------
    // 투사체 초기 설정
    //--------------------------------------------------------------------------
    public void Initialize(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, AttackStatsProvider stats)
    {
        _points[0] = p1;
        _points[1] = p2;
        _points[2] = p3;
        _points[3] = p4;
        _stats = stats;
        _timer = 0f;
    }

    //--------------------------------------------------------------------------
    // 기본 상태 설정
    //--------------------------------------------------------------------------
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);

        if (_stats == null)
        {
            _stats = ownerWeapon.GetComponentInParent<AttackStatsProvider>();
        }

        float range = 1f;
        float speed = 1f;
        if (_stats != null)
        {
            range = _stats.GetAttackRange(ownerWeapon);
            speed = _stats.GetAttackSpeed(ownerWeapon);
        }

        // 이동 시간 계산 (대상까지 거리 / 속도)
        _travelTime = speed > 0f ? range / speed : 1f;
        RemainingLifeTime = _travelTime;
        _timer = 0f;
    }

    //--------------------------------------------------------------------------
    // 매 프레임 위치 갱신
    //--------------------------------------------------------------------------
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        base.Update();
    }

    //--------------------------------------------------------------------------
    // 베지어 곡선을 따라 이동
    //--------------------------------------------------------------------------
    protected override void Move()
    {
        if (_stats == null)
        {
            _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
            if (_stats == null) return;
        }

        _timer += Time.deltaTime;
        float t = _travelTime > 0f ? Mathf.Clamp01(_timer / _travelTime) : 1f;

        Vector3 pos = CalculateBezier(_points[0], _points[1], _points[2], _points[3], t);
        transform.position = pos;
    }

    //--------------------------------------------------------------------------
    // 생존 시간 초과 시 사망 처리
    //--------------------------------------------------------------------------
    public override void CheckDead()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        UpdateCameraBound();

        if (_timer >= _travelTime)
        {
            Dead();
        }
    }

    //--------------------------------------------------------------------------
    // 베지어 계산 함수
    //--------------------------------------------------------------------------
    private static Vector3 CalculateBezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float value)
    {
        Vector3 a = Vector3.Lerp(p1, p2, value);
        Vector3 b = Vector3.Lerp(p2, p3, value);
        Vector3 c = Vector3.Lerp(p3, p4, value);

        Vector3 d = Vector3.Lerp(a, b, value);
        Vector3 e = Vector3.Lerp(b, c, value);

        return Vector3.Lerp(d, e, value);
    }
}

