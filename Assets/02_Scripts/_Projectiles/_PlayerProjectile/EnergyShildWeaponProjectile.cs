using System.Collections.Generic;
using UnityEngine;

public class EnergyShildWeaponProjectile : ProjectileBase, IPushable
{
    private AttackStatsProvider _stats;
    private Transform _center;
    private float _spawnTime;
    private float _lifeTime;
    private ParticleSystem _particle;

    public void Initialize(AttackStatsProvider stats, float lifeTime)
    {
        _stats = stats;
        _center = OwnerWeapon.transform.parent != null ? OwnerWeapon.transform.parent : OwnerWeapon.transform;
        _spawnTime = Time.time;
        _lifeTime = lifeTime;
        _particle = GetComponent<ParticleSystem>();
        UpdateParticleSize();
    }

    public override void SetStatus(WeaponBase ownerWeapon)
    {
        base.SetStatus(ownerWeapon);
        DisableType = WeaponDisableType.WEAPONDISABLETYPE_TIME;
    }

    protected override void Update()
    {
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;
        Move();
        DoAttack();
        CheckDead();
    }

    protected override void Move()
    {
        base.Move();
        if (_center != null) transform.position = _center.position;
    }

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
                    enemy.transform.position += delta.normalized * knock;
                }
            }
        }
    }

    private void UpdateParticleSize()
    {
        if (_particle == null || _stats == null) return;
        float range = _stats.GetAttackRange(OwnerWeapon);
        var main = _particle.main;
        main.startSize = new ParticleSystem.MinMaxCurve(range);
    }

    public void Push(Vector3 direction)
    {
        // Shield does not move by push
    }

    public override void CheckDead()
    {
        if (Time.time >= _spawnTime + _lifeTime)
        {
            Dead();
        }
    }
}
