using UnityEngine;

//==============================================================================
// GatlingWeapon
//==============================================================================
// 매우 짧은 쿨타임으로 연사되는 개틀링 무기. PistolWeapon과 구조는 유사하지만
// 탄환이 작은 각도로 퍼져나가도록 방향에 무작위 편차를 준다.
//==============================================================================

public class GatlingWeapon : WeaponBase
{
    // 발사할 투사체 데이터를 저장한다
    [SerializeField]
    private ProjectileBaseSO _projectileData;

    // 스탯 계산을 담당할 프로바이더
    private AttackStatsProvider _stats;

    // 발사 간격을 제어하기 위한 타이머
    private float _cooldownTimer;

    // 발사 방향을 결정하기 위한 플레이어 마우스 방향 컨트롤러
    private PlayerMouseDirectionController _directionCtrl;

    // 탄이 퍼지는 최대 각도(양쪽 합산). 너무 넓지 않도록 기본값은 15도 정도로 둔다
    [SerializeField]
    private float _spreadAngle = 15f;

    //--------------------------------------------------------------------------
    // 무기 세팅
    //--------------------------------------------------------------------------
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
            _projectileData = AllManager.instance.ProjectileFactory.GetProjectileData(WeaponType);
        }

        if (_directionCtrl == null && transform.parent != null)
        {
            _directionCtrl = transform.parent.GetComponent<PlayerMouseDirectionController>();
        }

        _cooldownTimer = 0f;
    }

    //--------------------------------------------------------------------------
    // 매 프레임 호출되는 갱신 로직
    //--------------------------------------------------------------------------
    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

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
            // 첫 발사는 바로 실행한다
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    //--------------------------------------------------------------------------
    // 실제 발사 로직
    //--------------------------------------------------------------------------
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        int count = _stats.GetProjectileCount(this);

        // 기본 발사 방향을 구한다. 컨트롤러가 없으면 자신의 우측 방향을 사용
        Vector3 baseDir = transform.right;
        if (_directionCtrl != null)
        {
            _directionCtrl.Rotate();
            if (_directionCtrl.RotationDirection.sqrMagnitude > 0f)
            {
                baseDir = _directionCtrl.RotationDirection;
            }
        }

        for (int i = 0; i < count; i++)
        {
            // 퍼짐 각도 범위 내에서 무작위 편차를 구한다
            float half = _spreadAngle * 0.5f;
            float randAngle = Random.Range(-half, half);
            Vector3 dir = Quaternion.Euler(0f, 0f, randAngle) * baseDir;

            var proj = AllManager.instance.ProjectileFactory.Create(_projectileData, transform.position, Quaternion.identity, this) as GatlingWeaponProjectile;
            if (proj != null)
            {
                proj.Initialize(dir, _stats);
            }
            else
            {
                Debug.LogError("Failed to create GatlingWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}

