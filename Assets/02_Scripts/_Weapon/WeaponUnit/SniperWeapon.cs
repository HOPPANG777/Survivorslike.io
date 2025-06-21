using UnityEngine;

//==============================================================================
// SniperWeapon
//==============================================================================
// 마우스가 가리키는 방향으로 강력한 탄환을 발사하는 스나이퍼 무기.
// 발사 시 반동으로 플레이어가 뒤로 밀려난다. 나머지 동작은 PistolWeapon과 유사하다.
//==============================================================================

public class SniperWeapon : WeaponBase
{
    // 발사에 사용될 투사체 데이터
    [SerializeField]
    private ProjectileBaseSO _projectileData;

    // 공격 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;

    // 발사 간격 관리를 위한 쿨다운 타이머
    private float _cooldownTimer;

    // 플레이어 방향 계산용 컨트롤러
    private PlayerMouseDirectionController _directionCtrl;

    // 발사 반동 거리
    [SerializeField]
    private float _recoilDistance = 0.5f;

    //--------------------------------------------------------------------------
    // 무기 초기 설정
    //--------------------------------------------------------------------------
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null) _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);

        if (_directionCtrl == null && transform.parent != null) _directionCtrl = transform.parent.GetComponent<PlayerMouseDirectionController>();

        _cooldownTimer = 0f;
    }

    //--------------------------------------------------------------------------
    // 매 프레임 호출되는 업데이트 로직
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
            // 첫 발사는 즉시 진행
            Shoot();
            _cooldownTimer += Time.deltaTime;
        }
    }

    //--------------------------------------------------------------------------
    // 투사체 발사 및 반동 적용
    //--------------------------------------------------------------------------
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        int count = _stats.GetProjectileCount(this);

        // 발사 방향 계산 (없으면 자신의 우측 방향)
        Vector3 dir = transform.right;
        if (_directionCtrl != null)
        {
            _directionCtrl.Rotate();  // 방향 갱신
            if (_directionCtrl.RotationDirection.sqrMagnitude > 0f)
            {
                dir = _directionCtrl.RotationDirection;
            }
        }

        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create(_projectileData, transform.position, Quaternion.identity, this) as SniperWeaponProjectile;
            if (proj != null)
            {
                proj.Initialize(dir, _stats);
            }
            else
            {
                Debug.LogError("Failed to create SniperWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }

        // 발사 후 반동으로 플레이어를 뒤로 이동시킴
        if (transform.parent != null)
        {
            transform.parent.position -= dir.normalized * _recoilDistance;
        }
    }
}
