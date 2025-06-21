using UnityEngine;

// 벽에 반사되는 레이저 무기

public class LaserWeapon : WeaponBase
{
    // 레이저 투사체 데이터
    [SerializeField]
    private ProjectileBaseSO _projectileData;

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;
    // 공격 쿨타임 타이머
    private float _cooldownTimer;

    // 무기 사용을 위한 기본 설정
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null) _projectileData = AllManager.instance.ProjectileFactory.GetProjectileData(WeaponType);

        _cooldownTimer = 0f;
    }

    // 매 프레임 호출되어 레이저 발사를 관리한다
    public override void Update()
    {
        base.Update();

        if (_stats == null) return;
        _stats.SetTargetWeapon(this);

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
            // 첫 발사는 즉시 수행
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    // 레이저 투사체를 발사한다
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        int count = _stats.GetProjectileCount(this);
        for (int i = 0; i < count; i++)
        {
            var proj = AllManager.instance.ProjectileFactory.Create(_projectileData, transform.position, Quaternion.identity, this) as LaserWeaponProjectile;
            if (proj != null)
            {
                // 무작위 방향으로 레이저 발사
                Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                proj.Initialize(dir, _stats);
            }
            else
            {
                Debug.LogError("Failed to create LaserWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}
