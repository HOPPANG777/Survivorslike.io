using UnityEngine;

public class AegisWeaponProjectile : ProjectileBase
{
    // 눈에 띄는 생성/소멸 이펙트를 위해 기존보다 빠르고 크게 스케일을 변화시킨다.
    private const float ScaleDuration = 0.1f;
    private const float TargetScale = 1.2f;

    private AttackStatsProvider _stats;
    private Transform _center;
    private float _scaleTimer;
    private bool _shrinking;
    private float _shrinkTimer;

    public int Index { get; private set; }

    public int MaxCount { get; private set; }

    public void Initialize(int index, int maxCount)
    {
        Index = index;
        MaxCount = maxCount;
        _center = OwnerWeapon.transform.parent != null ? OwnerWeapon.transform.parent : OwnerWeapon.transform;
        _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
        transform.localScale = Vector3.zero;
        _scaleTimer = 0f;
        _shrinking = false;
        _shrinkTimer = 0f;
    }

    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.Update();
        UpdateScale();
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        // DisableType를 직접 제어하기 위해 NONE으로 설정
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_NONE;
    }

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

    private void UpdateScale()
    {
        if (_shrinking)
        {
            _shrinkTimer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(_shrinkTimer / ScaleDuration);
            transform.localScale = Vector3.one * TargetScale * t;
            if (_shrinkTimer >= ScaleDuration) Dead();
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

    public void StartShrink()
    {
        if (_shrinking) return;
        _shrinking = true;
        _shrinkTimer = 0f;
        // 수명 카운트를 초기화하여 즉시 사라지는 것을 방지
        RemainingLifeTime = ScaleDuration;
        _age = 0f;
    }

    public override void SoundPlay()
    {
        base.SoundPlay();
    }

    public override void SoundStop()
    {
        base.SoundStop();
    }

    public override void CheckDead()
    {
        base.CheckDead();
        if (_shrinking)
        {
            // 카메라 밖으로 나갔는지 체크는 유지
            UpdateCameraBound();
            return;
        }

        // base.CheckDead()에서 바로 Dead()가 호출되어 축소 효과가 생략되는 경우가 있어
        // 직접 조건을 검사하여 축소 과정을 거치도록 한다.
        UpdateCameraBound();

        switch (DisableType)
        {
            case WeaponDisableType.WEAPONDISABLETYPE_NONE:
                _age += Time.deltaTime;
                if (_age < RemainingLifeTime) return;
                StartShrink();
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_COUNT:
                if (RemainingLifeTime > 0) return;
                StartShrink();
                break;
            case WeaponDisableType.WEAPONDISABLETYPE_TIME:
                if (RemainingPierceCount > 0) return;
                StartShrink();
                break;
        }
    }

    public override void Dead()
    {
        base.Dead();
    }
}
