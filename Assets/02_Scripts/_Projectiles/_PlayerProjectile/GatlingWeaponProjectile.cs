using UnityEngine;

//==============================================================================
// GatlingWeaponProjectile
//==============================================================================
// GatlingWeapon에서 사용되는 기본 탄환. PistolWeaponProjectile과 거의 동일하
// 지만, 개틀링 무기의 빠른 연사를 고려하여 간단하게 구현한다.
//==============================================================================

public class GatlingWeaponProjectile : ProjectileBase
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

        // 방향에 맞춰 회전값을 조정한다
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
        base.Update();
    }

    //--------------------------------------------------------------------------
    // 지정된 방향으로 이동
    //--------------------------------------------------------------------------
    protected override void Move()
    {
        if (_stats == null)
        {
            // 프로바이더가 없다면 발사한 무기에서 가져온다
            _stats = OwnerWeapon.GetComponentInParent<AttackStatsProvider>();
            if (_stats == null)
            {
                return;
            }
        }

        transform.position += _direction * _stats.GetAttackSpeed(OwnerWeapon) * Time.deltaTime;
    }
}

