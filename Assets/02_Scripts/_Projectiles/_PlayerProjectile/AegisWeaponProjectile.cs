using UnityEngine;

public class AegisWeaponProjectile : ProjectileBase
{
    private const float ScaleDuration = 0.2f;

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
        if (_stats != null) _stats.SetTargetWeapon(OwnerWeapon);
        _shrinkTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();
        Move();
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
        if (_center == null || _stats == null) return;
        if (_stats != null) _stats.SetTargetWeapon(OwnerWeapon);
        float angle = Time.time * _stats.AttackSpeed * 360f;
        float rad = Mathf.Deg2Rad * (angle + Index * (360f / MaxCount));
        float range = _stats.AttackRange;
        Vector3 offset = new Vector3(Mathf.Sin(rad) * range, Mathf.Cos(rad) * range, 0f);
        transform.position = _center.position + offset;
    }

    private void UpdateScale()
    {
        if (_shrinking)
        {
            _shrinkTimer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(_shrinkTimer / ScaleDuration);
            transform.localScale = Vector3.one * t;
            if (_shrinkTimer >= ScaleDuration) Dead();
        }
        else
        {
            if (_scaleTimer < ScaleDuration)
            {
                _scaleTimer += Time.deltaTime;
                float t = Mathf.Clamp01(_scaleTimer / ScaleDuration);
                transform.localScale = Vector3.one * t;
            }
        }
    }

    public void StartShrink()
    {
        if (_shrinking) return;
        _shrinking = true;
        _shrinkTimer = 0f;
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
    }

    public override void Dead()
    {
        base.Dead();
    }
}
