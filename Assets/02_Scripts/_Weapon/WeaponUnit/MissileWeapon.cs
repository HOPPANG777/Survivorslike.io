using UnityEngine;

public class MissileWeapon : WeaponBase
{
    // 투사체 데이터
    [SerializeField]
    private ProjectileBaseSO _projectileData;

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;
    // 쿨다운 측정용 타이머
    private float _cooldownTimer;

    // 초기 설정
    public override void SetStatus()
    {
        base.SetStatus();
        if (_stats == null)
        {
            _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();
        }
        _stats.SetTargetWeapon(this);
        if (_projectileData == null)
        {
            _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);
        }
        _cooldownTimer = 0f;
    }

    // 매 프레임 호출
    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME)
        {
            return;
        }
        base.Update();

        if (_stats != null)
        {
            _stats.SetTargetWeapon(this);
        }

        if (_cooldownTimer > 0f)
        {
            _cooldownTimer += Time.deltaTime;
            if (_stats != null && _cooldownTimer >= _stats.GetAttackCooldown(this))
            {
                _cooldownTimer = 0f;
                Shoot();
            }
        }
        else
        {
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    // 투사체 발사
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null)
        {
            return;
        }

        int count = Mathf.Max(1, _stats.GetProjectileCount(this));
        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create(_projectileData, transform.position, Quaternion.identity, this) as MissileWeaponProjectile;
            if (proj != null)
            {
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                proj.Initialize(dir, _stats);
            }
            else
            {
                Debug.LogError("Failed to create MissileWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}
