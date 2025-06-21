using UnityEngine;

// 곡선을 따라 투척되는 폭탄 무기

public class BombWeapon : WeaponBase
{
    // 폭탄 투사체의 정보를 가진 SO
    [SerializeField] private ProjectileBaseSO _projectileData;

    // 공격 스탯 계산을 위한 프로바이더
    private AttackStatsProvider _stats;
    // 공격 쿨타임 측정을 위한 타이머
    private float _cooldownTimer;
    // 투척 방향을 번갈아가며 결정하기 위한 플래그
    private bool _leftRightToggle;

    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null) _projectileData = AllManager.instance.ProjectileFactory.GetProjectileData(WeaponType);

        _cooldownTimer = 0f;

        _leftRightToggle = false;
    }

    // 매 프레임 호출되어 투사체 발사를 제어한다
    public override void Update()
    {
        base.Update();
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        if (_stats == null) return;
        _stats.SetTargetWeapon(this);

        if (_cooldownTimer > 0f)
        {
            // 쿨타임 경과 여부 확인
            _cooldownTimer += Time.deltaTime;
            if (_stats != null && _cooldownTimer >= _stats.GetAttackCooldown(this))
            {
                _cooldownTimer = 0f;
                Shoot();
            }
        }
        else
        {
            // 첫 발사는 바로 실행
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    // 폭탄 투사체를 생성하여 곡선 경로로 날린다
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        int count = _stats.GetProjectileCount(this);
        for (int i = 0; i < count; i++)
        {
            var proj = AllManager.instance.ProjectileFactory.Create( _projectileData, transform.position, Quaternion.identity, this) as BombWeaponProjectile;
            if (proj != null)
            {
                // 베지어 곡선의 네 지점을 계산하여 초기화
                Vector3 p1 = transform.position;
                Vector3 p2 = p1 + new Vector3(0f, 5f, 0f);

                float x1 = UnityEngine.Random.Range(1f, 5f);
                if (_leftRightToggle) x1 = -x1;

                Vector3 p3 = p2 + new Vector3(x1, 0f, 0f);
                Vector3 p4 = p1 + new Vector3(x1, -10f, 0f);
                proj.Initialize(p1, p2, p3, p4);
                _leftRightToggle = !_leftRightToggle;
            }
            else
            {
                Debug.LogError("Failed to create BombWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}
