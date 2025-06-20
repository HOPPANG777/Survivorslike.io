using System.Collections.Generic;
using UnityEngine;

// 적을 추적하는 미사일 투사체

public class MissileWeaponProjectile : ProjectileBase
{
    // 현재 방향
    private Vector3 _direction;
    // 스탯 프로바이더
    private AttackStatsProvider _stats;
    // 추적 대상
    private EnemyUnit _target;

    // 초기 각도 설정
    public void Initialize(Vector3 direction, AttackStatsProvider stats)
    {
        _direction = direction.normalized;
        _stats = stats;
        _target = null;
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
    }

    protected override void Update()
    {
        base.Update();
    }

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

        float range = _stats.GetAttackRange(OwnerWeapon);
        float sqrRange = range * range;

        // 타겟 갱신 조건 검사
        if (_target == null || _target.IsDead || ( _target.transform.position - transform.position ).sqrMagnitude > sqrRange)
        {
            _target = FindTarget(range);
        }

        // 타겟이 있으면 목표 방향으로 서서히 회전
        if (_target != null)
        {
            Vector3 toTarget = (_target.transform.position - transform.position).normalized;
            float strength = Attributes.HomingStrength;
            _direction = Vector3.Lerp(_direction, toTarget, strength * Time.deltaTime).normalized;
        }

        transform.position += _direction * _stats.GetAttackSpeed(OwnerWeapon) * Time.deltaTime;
    }

    private EnemyUnit FindTarget(float range)
    {
        List<ICollidable> list = AllManager.instance.CollisionManager.GetAllCollidables();
        Vector3 pos = transform.position;
        float minSqr = float.MaxValue;
        EnemyUnit closest = null;
        float sqrRange = range * range;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (list[i] is EnemyUnit enemy)
            {
                if (enemy.IsDead) continue;
                Vector3 delta = enemy.transform.position - pos;
                float sqr = delta.sqrMagnitude;
                if (sqr <= sqrRange && sqr < minSqr)
                {
                    minSqr = sqr;
                    closest = enemy;
                }
            }
        }
        return closest;
    }
}
