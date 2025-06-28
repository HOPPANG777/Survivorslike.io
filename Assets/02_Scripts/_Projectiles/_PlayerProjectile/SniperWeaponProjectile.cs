using UnityEngine;

//==============================================================================
// SniperWeaponProjectile
//==============================================================================
// SniperWeapon에서 발사되는 탄환. 지정된 방향으로 고속 이동한다.
// 기타 동작은 PistolWeaponProjectile과 동일하다.
//==============================================================================

public class SniperWeaponProjectile : ProjectileBase
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

        // 방향에 맞춰 회전
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
            _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
            if (_stats == null)
            {
                return;
            }
        }

        transform.position += _direction * _stats.GetAttackSpeed(OwnerWeapon) * Time.deltaTime;
    }
}
