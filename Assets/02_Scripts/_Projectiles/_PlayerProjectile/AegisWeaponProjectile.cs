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
    private float _spawnTime;
    private float _lifeTime;
    private float _shrinkStartTime;

    public int Index { get; private set; }

    public int MaxCount { get; private set; }

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

    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        CheckDead();
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

    public override void Dead()
    {
        base.Dead();
    }
}
