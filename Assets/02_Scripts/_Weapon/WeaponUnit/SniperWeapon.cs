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

    // 투사체 경로 미리보기 오브젝트
    [SerializeField]
    private GameObject _pathPreviewObj;
    private IProjectilePathPreviewer _pathPreview;

    //--------------------------------------------------------------------------
    // 무기 초기 설정
    //--------------------------------------------------------------------------
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null) _projectileData = AllManager.instance.ProjectileFactory.GetProjectileData(WeaponType);

        if (_directionCtrl == null && transform.parent != null) _directionCtrl = transform.parent.GetComponent<PlayerMouseDirectionController>();

        _cooldownTimer = 0f;

        // 경로 미리보기 초기화 및 표시
        if (_pathPreviewObj != null)
        {
            _pathPreview = _pathPreviewObj.GetComponent<SniperPathPreview>() as IProjectilePathPreviewer;
            if (_pathPreview != null)
            {
                _pathPreview.Initialize(transform);
                float range = _stats != null ? _stats.GetAttackRange(this) : 0f;
                _pathPreview.UpdatePath(transform.right, range);
                _pathPreview.Show();
            }
        }
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

        // 현재 방향을 이용해 미리보기 갱신
        Vector3 previewDir = transform.right;
        if (_directionCtrl != null)
        {
            _directionCtrl.Rotate();
            if (_directionCtrl.RotationDirection.sqrMagnitude > 0f)
            {
                previewDir = _directionCtrl.RotationDirection;
            }
        }
        if (_pathPreview != null)
        {
            float range = _stats.GetAttackRange(this);
            _pathPreview.UpdatePath(previewDir, range);
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
            var proj = AllManager.instance.ProjectileFactory.Create(_projectileData, transform.position, Quaternion.identity, this) as SniperWeaponProjectile;
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
