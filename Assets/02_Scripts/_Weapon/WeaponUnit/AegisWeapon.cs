using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어 주변을 돌며 공격하는 에이직 방패 무기

// 모든 변수와 함수에 한글 설명 주석을 추가하여 무기의 동작을 이해하기 쉽게 한다.

public class AegisWeapon : WeaponBase
{
    // 투사체의 기본 데이터를 들고 있는 SO
    [TabGroup("Agies Weapon")][Tooltip("Projectile Data")][SerializeField]
    private ProjectileBaseSO _projectileData;

    // 현재 활성화되어 있는 방패 투사체 목록
    [TabGroup("Agies Weapon")][Tooltip("_activeProjectiles")][SerializeField]
    private readonly List<AegisWeaponProjectile> _activeProjectiles = new();

    // 공격 스탯 계산을 담당하는 프로바이더
    [TabGroup("Agies Weapon")][Tooltip("_stats")][SerializeField]
    private AttackStatsProvider _stats;

    // 공격 사이의 쿨타임을 측정하는 타이머
    [TabGroup("Agies Weapon")][Tooltip("_cooldownTimer")][SerializeField]
    private float _cooldownTimer;

    // 투사체 유지 시간을 체크하기 위한 타이머
    [TabGroup("Agies Weapon")][Tooltip("_durationTimer")][SerializeField]
    private float _durationTimer;

    // 축소 애니메이션에 사용되는 고정 시간
    [TabGroup("Agies Weapon")][Tooltip("ShrinkDuration")][SerializeField]
    private const float ShrinkDuration = 0.1f;

    // 투사체가 축소 중인지 여부
    [TabGroup("Agies Weapon")][Tooltip("_isShrinking")][SerializeField]
    private bool _isShrinking;

    // 축소 애니메이션 진행 시간을 담는 타이머
    [TabGroup("Agies Weapon")][Tooltip("_shrinkTimer")][SerializeField]
    private float _shrinkTimer;

    // 이전에 생성된 투사체 수를 기록하여 변경 감지를 위한 값
    [TabGroup("Agies Weapon")][Tooltip("_lastProjectileCount")][SerializeField]
    private int _lastProjectileCount = -1;

    // 무기의 기본 상태를 세팅한다
    public override void SetStatus()
    {
        base.SetStatus();

        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();

        _stats.SetTargetWeapon(this);

        // 각종 타이머 초기화
        _cooldownTimer = 0f;

        _durationTimer = 0f;

        _isShrinking = false;

        _shrinkTimer = 0f;

        transform.localScale = Vector3.one;

        // 투사체 데이터가 비어 있다면 팩토리에서 가져온다
        if (_projectileData == null) _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);

        _lastProjectileCount = Mathf.Max(1, _stats.GetProjectileCount(this));
    }

    // 매 프레임 호출되는 업데이트 로직
    public override void Update()
    {
        // 인게임 상태가 아니면 동작하지 않는다
        base.Update();

        // 축소 중이면 축소 애니메이션을 진행한다
        if (_isShrinking)
        {
            _shrinkTimer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(_shrinkTimer / ShrinkDuration);
            transform.localScale = Vector3.one * t;
            if (_shrinkTimer >= ShrinkDuration) base.Dead();
            return;
        }

        if (_projectileData == null || _stats == null) return;

        // 스탯 프로바이더에 자신을 세팅하여 최신 값을 유지
        _stats.SetTargetWeapon(this);

        // 현재 설정된 투사체 개수를 확인하여 변경 시 재생성한다
        int currentCount = Mathf.Max(1, _stats.GetProjectileCount(this));
        if (currentCount != _lastProjectileCount)
        {
            ReleaseProjectile();
            _cooldownTimer = 0f;
            _durationTimer = 0f;
            _lastProjectileCount = currentCount;
        }

        if (_durationTimer > 0f)
        {
            _durationTimer -= Time.deltaTime;
            if (_durationTimer <= 0f) ReleaseProjectile(); // 지속시간 종료 시 투사체 회수
        }
        else
        {
            // 처음 공격은 바로 발사한다
            if (_cooldownTimer == 0f)
            {
                Shoot();
                _cooldownTimer += Time.deltaTime;
            }
            else
            {
                _cooldownTimer += Time.deltaTime;
                if (_stats != null && _cooldownTimer >= _stats.GetAttackCooldown(this))
                {
                    _cooldownTimer = 0f;
                    Shoot();              // 쿨타임마다 발사
                }
            }
        }
    }

    // 투사체를 생성하여 배치한다
    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        ReleaseProjectile();
        int count = Mathf.Max(1, _stats.GetProjectileCount(this));
        float spawnTime = Time.time;
        float lifeTime = _stats.GetAttackDuration(this);
        // 요구 개수만큼 투사체를 생성해 원형 배치한다
        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create( _projectileData, transform.position, Quaternion.identity, this.gameObject.GetComponent<WeaponBase>()) as AegisWeaponProjectile;
            if (proj != null)
            {
                proj.Initialize(i, count, spawnTime, lifeTime);
                _activeProjectiles.Add(proj);
            }
            else
            {
                Debug.LogError("Failed to create AegisWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
        _durationTimer = lifeTime;
        _lastProjectileCount = count;

        //if (!string.IsNullOrEmpty(SoundName)) SoundManager.instance.PlaySound(SoundName);
    }

    // 활성화 중인 투사체를 모두 축소시킨다
    public override void ReleaseProjectile()
    {
        for (int i = 0; i < _activeProjectiles.Count; i++)
        {
            if (_activeProjectiles[i] != null) _activeProjectiles[i].StartShrink();
        }
        _activeProjectiles.Clear();
    }

    // 무기 파괴 시 호출되는 처리
    public override void Dead()
    {
        if (_isShrinking) return;
        ReleaseProjectile();
        _isShrinking = true;
        _shrinkTimer = 0f;
    }
}
