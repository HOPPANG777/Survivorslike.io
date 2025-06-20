using UnityEngine;

// 플레이어를 감싸는 에너지 실드 무기

public class EnergyShildWeapon : WeaponBase
{
    [SerializeField] private ProjectileBaseSO _projectileData;

    private AttackStatsProvider _stats;
    private float _cooldownTimer;
    private float _durationTimer;
    private EnergyShildWeaponProjectile _activeProjectile;

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

    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.Update();
        if (_projectileData == null || _stats == null) return;
        if (_stats != null) _stats.SetTargetWeapon(this);

        if (_durationTimer > 0f)
        {
            _durationTimer -= Time.deltaTime;
            if (_durationTimer <= 0f) ReleaseProjectile();
        }
        else
        {
            if (_cooldownTimer == 0f)
            {
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

    public override void ReleaseProjectile()
    {
        if (_activeProjectile != null)
        {
            _activeProjectile.Dead();
            _activeProjectile = null;
        }
    }

    public override void Dead()
    {
        ReleaseProjectile();
        base.Dead();
    }
}
