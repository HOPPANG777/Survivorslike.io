using UnityEngine;

//==============================================================================
// PistolWeapon
//==============================================================================
// PlayerMouseDirectionController가 가리키는 방향으로 탄환을 발사하는 기본형
// 권총 무기. 쿨다운에 맞춰 자동으로 발사한다.
//==============================================================================

public class PistolWeapon : WeaponBase
{
    // 발사에 사용될 투사체 데이터
    [SerializeField]
    private ProjectileBaseSO _projectileData;

    // 공격력, 속도 등을 계산하기 위한 스탯 프로바이더
    private AttackStatsProvider _stats;

    // 발사 간격 제어용 쿨다운 타이머
    private float _cooldownTimer;

    // 방향 계산을 위해 사용할 플레이어 마우스 방향 컨트롤러
    private PlayerMouseDirectionController _directionCtrl;

    //---------------------------------------------------------------------------
    // 무기 초기화
    //---------------------------------------------------------------------------
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null)
            _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        if (_projectileData == null)
            _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);

        // 부모 객체에서 방향 컨트롤러를 찾아둔다
        if (_directionCtrl == null && transform.parent != null)
            _directionCtrl = transform.parent.GetComponent<PlayerMouseDirectionController>();

        _cooldownTimer = 0f;
    }

    //---------------------------------------------------------------------------
    // 매 프레임 호출되는 업데이트 로직
    //---------------------------------------------------------------------------
    public override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME)
            return;

        base.Update();

        if (_stats != null)
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

    //---------------------------------------------------------------------------
    // 투사체 발사
    //---------------------------------------------------------------------------
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null)
            return;

        int count = Mathf.Max(1, _stats.GetProjectileCount(this));

        // 현재 바라보는 방향을 구한다. 컨트롤러가 없으면 자신의 Right 방향 사용
        Vector3 dir = transform.right;
        if (_directionCtrl != null)
            dir = _directionCtrl.RotationDirection;

        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create(
                _projectileData,
                transform.position,
                Quaternion.identity,
                this) as PistolWeaponProjectile;

            if (proj != null)
            {
                proj.Initialize(dir, _stats);
            }
            else
            {
                Debug.LogError("Failed to create PistolWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
    }
}
