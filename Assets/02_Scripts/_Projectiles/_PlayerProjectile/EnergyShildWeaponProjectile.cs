using System.Collections.Generic;
using UnityEngine;

// 플레이어를 둘러싸는 에너지 실드 투사체

public class EnergyShildWeaponProjectile : ProjectileBase, IPushable
{
    // 스탯 계산용 프로바이더
    private AttackStatsProvider _stats;
    // 중심이 될 트랜스폼
    private Transform _center;
    // 생성 시점
    private float _spawnTime;
    // 유지 시간
    private float _lifeTime;
    // 실드 이펙트 파티클
    private ParticleSystem _particle;

    // 투사체 초기화
    public void Initialize(AttackStatsProvider stats, float lifeTime)
    {
        _stats = stats;
        _center = OwnerWeapon.transform.parent != null ? OwnerWeapon.transform.parent : OwnerWeapon.transform;
        _spawnTime = Time.time;
        _lifeTime = lifeTime;
        _particle = GetComponent<ParticleSystem>();
        UpdateParticleSize();
    }

    // 기본 상태 설정
    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_TIME;
    }

    // 매 프레임 공격 및 이동 처리
    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        DoAttack();
        CheckDead();
    }

    // 플레이어 위치에 맞춰 이동
    protected override void Move()
    {
        base.Move();
        if (_center != null) transform.position = _center.position;
    }

    // 범위 내 적에게 데미지와 넉백을 적용
    private void DoAttack()
    {
        if (_stats == null) return;

        float range = _stats.GetAttackRange(OwnerWeapon);
        float knock = _stats.GetKnockbackForce(OwnerWeapon);
        UpdateParticleSize();

        List<ICollidable> list = AllManager.instance.CollisionManager.GetAllCollidables();
        Vector3 myPos = _center != null ? _center.position : transform.position;
        float sqrRange = range * range;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] is EnemyUnit enemy)
            {
                Vector3 delta = enemy.transform.position - myPos;
                if (delta.sqrMagnitude <= sqrRange)
                {
                    enemy.Hitted(AttackPower);
                    enemy.transform.position += delta.normalized * knock * Time.deltaTime;
                }
            }
        }
    }

    // 파티클 크기를 무기 범위에 맞게 조정
    private void UpdateParticleSize()
    {
        if (_particle == null || _stats == null) return;
        float range = _stats.GetAttackRange(OwnerWeapon);
        var main = _particle.main;
        main.startSize = new ParticleSystem.MinMaxCurve(range);
    }

    // 실드는 밀려나지 않으므로 빈 구현
    public void Push(Vector3 direction)
    {
        // Shield does not move by push
    }

    // 유지 시간이 끝나면 소멸
    public override void CheckDead()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        if (Time.time >= _spawnTime + _lifeTime)
        {
            Dead();
        }
    }
}
