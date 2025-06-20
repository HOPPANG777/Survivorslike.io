using UnityEngine;

// 에이직 무기의 방패 투사체

public class AegisWeaponProjectile : ProjectileBase
{
    // 생성/소멸 시 사용되는 스케일 애니메이션 지속 시간
    private const float ScaleDuration = 0.1f;
    // 최대 스케일 값
    private const float TargetScale = 1.2f;

    // 스탯 계산을 위한 프로바이더
    private AttackStatsProvider _stats;
    // 원형 이동의 중심 위치
    private Transform _center;
    // 스케일 애니메이션 진행 시간
    private float _scaleTimer;
    // 축소 상태 여부
    private bool _shrinking;
    // 생성 시점
    private float _spawnTime;
    // 생존 시간
    private float _lifeTime;
    // 축소 시작 시점
    private float _shrinkStartTime;

    public int Index { get; private set; }

    public int MaxCount { get; private set; }

    // 투사체 초기 설정
    public void Initialize(int index, int maxCount, float spawnTime, float lifeTime)
    {
        Index = index;
        MaxCount = maxCount;
        _center = OwnerWeapon.transform.parent != null ? OwnerWeapon.transform.parent : OwnerWeapon.transform;
        _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
        transform.localScale = Vector3.zero;
        _scaleTimer = 0f;
        _shrinking = false;
        _spawnTime = spawnTime;
        _lifeTime = lifeTime;
        _shrinkStartTime = 0f;
    }

    // 매 프레임 위치 이동 및 크기 갱신
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        CheckDead();
        UpdateScale();
    }

    // 기본 상태 설정
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        // DisableType를 직접 제어하기 위해 NONE으로 설정
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;
    }

    // 원을 그리며 회전하는 움직임 구현
    protected override void Move()
    {
        base.Move();
        if (_center == null || _stats == null) return;
        float angle = Time.time * _stats.GetAttackSpeed(OwnerWeapon) * 360f;
        float rad = Mathf.Deg2Rad * (angle + Index * (360f / MaxCount));
        float range = _stats.GetAttackRange(OwnerWeapon);
        Vector3 offset = new Vector3(Mathf.Sin(rad) * range, Mathf.Cos(rad) * range, 0f);
        transform.position = _center.position + offset;
    }

    // 스케일 애니메이션 처리
    private void UpdateScale()
    {
        if (_shrinking)
        {
            float elapsed = Time.time - _shrinkStartTime;
            float t = 1f - Mathf.Clamp01(elapsed / ScaleDuration);
            transform.localScale = Vector3.one * TargetScale * t;
            if (elapsed >= ScaleDuration) Dead();
        }
        else
        {
            if (_scaleTimer < ScaleDuration)
            {
                _scaleTimer += Time.deltaTime;
            }

            float t = Mathf.Clamp01(_scaleTimer / ScaleDuration);
            transform.localScale = Vector3.one * TargetScale * t;
        }
    }

    // 축소 애니메이션 시작
    public void StartShrink()
    {
        if (_shrinking) return;
        _shrinking = true;
        _shrinkStartTime = Time.time;
    }

    public override void SoundPlay()
    {
        base.SoundPlay();
    }

    public override void SoundStop()
    {
        base.SoundStop();
    }

    // 사망 조건 확인
    public override void CheckDead()
    {
        UpdateCameraBound();

        if (_shrinking)
        {
            return;
        }

        switch (DisableType)
        {
            case WeaponDisableType.WEAPONDISABLETYPE_NONE:
                if (Time.time < _spawnTime + _lifeTime) return;
                StartShrink();
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_COUNT:
                if (RemainingPierceCount > 0) return;
                StartShrink();
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_TIME:
                if (RemainingLifeTime > 0) return;
                StartShrink();
                break;
        }
    }

    // 즉시 제거 처리
    public override void Dead()
    {
        base.Dead();
    }
}
