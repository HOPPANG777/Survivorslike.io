using UnityEngine;

// 플레이어를 감싸는 에너지 실드 무기

public class EnergyShildWeapon : WeaponBase
{
    // 실드 투사체 데이터
    [SerializeField] private ProjectileBaseSO _projectileData;

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;
    // 공격 쿨타임 측정용 타이머
    private float _cooldownTimer;
    // 실드 지속 시간 측정용 타이머
    private float _durationTimer;
    // 현재 소환되어 있는 실드 투사체
    private EnergyShildWeaponProjectile _activeProjectile;

    // 무기 상태를 초기화한다
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null) _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);

        _cooldownTimer = 0f;

        _durationTimer = 0f;

        _activeProjectile = null;
    }

    // 실드 생성/유지 및 쿨타임을 관리한다
    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        base.Update();

        if (_projectileData == null || _stats == null) return;

        if (_stats == null) return;
        _stats.SetTargetWeapon(this);

        if (_durationTimer > 0f)
        {
            _durationTimer -= Time.deltaTime;
            if (_durationTimer <= 0f) ReleaseProjectile(); // 지속시간 종료 시 제거
        }
        else
        {
            if (_cooldownTimer == 0f)
            {
                // 첫 실드 생성
                Shoot();
                _cooldownTimer += Time.deltaTime;
            }
            else
            {
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _stats.GetAttackCooldown(this))
                {
                    _cooldownTimer = 0f;
                    Shoot();
                }
            }
        }
    }

    // 실드 투사체를 소환한다
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        ReleaseProjectile();
        var proj = ProjectileFactory.instance.Create(_projectileData, transform.position, Quaternion.identity, this) as EnergyShildWeaponProjectile;
        if (proj != null)
        {
            float lifeTime = _stats.GetAttackDuration(this);
            proj.Initialize(_stats, lifeTime);
            _activeProjectile = proj;
            _durationTimer = lifeTime;
        }
        else
        {
            Debug.LogError("Failed to create EnergyShildWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
        }
    }

    // 실드 투사체를 제거한다
    public override void ReleaseProjectile()
    {
        if (_activeProjectile != null)
        {
            _activeProjectile.Dead();
            _activeProjectile = null;
        }
    }

    // 무기가 파괴될 때 호출
    public override void Dead()
    {
        ReleaseProjectile();
        base.Dead();
    }
}
