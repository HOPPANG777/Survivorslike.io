using UnityEngine;

//==============================================================================
// PistolWeaponProjectile
//==============================================================================
// PistolWeapon에서 발사되는 기본 탄환. 발사 시 지정된 방향으로 일정 속도로
// 이동하며, 수명이나 화면 이탈 여부는 ProjectileBase에서 처리한다.
//==============================================================================

public class PistolWeaponProjectile : ProjectileBase
{
    // 현재 이동 방향
    private Vector3 _direction;

    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;

    //--------------------------------------------------------------------------
    // 투사체 초기 설정
    //--------------------------------------------------------------------------
    public void Initialize(Vector3 direction, AttackStatsProvider stats)
    {
        _direction = direction.normalized;
        _stats = stats;

        // 방향에 맞춰 회전 시켜준다
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    //--------------------------------------------------------------------------
    // 기본 상태 설정
    //--------------------------------------------------------------------------
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
    }

    //--------------------------------------------------------------------------
    // 매 프레임 이동 처리
    //--------------------------------------------------------------------------
    protected override void Update()
    {

        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        base.Update();
    }

    //--------------------------------------------------------------------------
    // 지정된 방향으로 직선 이동
    //--------------------------------------------------------------------------
    protected override void Move()
    {
        if (_stats == null)
        {
            // 스탯 프로바이더가 없으면 발사한 무기에서 가져온다
            _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
            if (_stats == null)
            {
                return;
            }
        }

        transform.position += _direction * _stats.GetAttackSpeed(OwnerWeapon) * Time.deltaTime;
    }
}
