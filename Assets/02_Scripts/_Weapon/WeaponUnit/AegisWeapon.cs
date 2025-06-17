using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AegisWeapon : WeaponBase
{
    [TabGroup("Agies Weapon")][Tooltip("Projectile Data")][SerializeField] private ProjectileBaseSO _projectileData;

    private readonly List<AegisWeaponProjectile> _activeProjectiles = new();
    private AttackStatsProvider _stats;
    private float _cooldownTimer;
    private float _durationTimer;
    private const float ShrinkDuration = 0.1f;
    private bool _isShrinking;
    private float _shrinkTimer;

    public override void SetStatus()
    {
        base.SetStatus();
        if (_stats == null) _stats = AllManager.instance.GamemodeManager.Player.GetComponent<AttackStatsProvider>();
        _stats.SetTargetWeapon(this);
        _cooldownTimer = 0f;
        _durationTimer = 0f;
        _isShrinking = false;
        _shrinkTimer = 0f;
        transform.localScale = Vector3.one;
        if (_projectileData == null) _projectileData = ProjectileFactory.instance.GetProjectileData(WeaponType);
    }

    public override void Update()
    {
        base.Update();
        if (_isShrinking) {
            _shrinkTimer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(_shrinkTimer / ShrinkDuration);
            transform.localScale = Vector3.one * t;
            if (_shrinkTimer >= ShrinkDuration) { base.Dead(); }
            return;
        }
        if (AllManager.instance.GamemodeManager.GameMode != GameModeType.GAMEMODE_INGAME) return;

        if (_stats != null) _stats.SetTargetWeapon(this);
        _stats.SetTargetWeapon(this);

        if (_durationTimer > 0f)
        {
            _durationTimer -= Time.deltaTime;
            if (_durationTimer <= 0f) ReleaseProjectile();
        }
        else
        {
            if (_cooldownTimer == 0f)
            {
                Shoot();                  // 일단 한 번 바로 쏨
                _cooldownTimer += Time.deltaTime;
            }
            else
            {
                _cooldownTimer += Time.deltaTime;
                if (_stats != null && _cooldownTimer >= _stats.AttackCooldown)
                {
                    _cooldownTimer = 0f;
                    Shoot();              // 쿨타임마다 쏨
                }
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        if (_projectileData == null || _stats == null) return;

        ReleaseProjectile();
        int count = Mathf.Max(1, _stats.ProjectileCount);
        for (int i = 0; i < count; i++)
        {
            var proj = ProjectileFactory.instance.Create(_projectileData, transform.position, Quaternion.identity, this.gameObject.GetComponent<WeaponBase>()) as AegisWeaponProjectile;
            if (proj != null)
            {
                proj.Initialize(i, count);
                _activeProjectiles.Add(proj);
            }
            else if (proj == null)
            {
                Debug.LogError("Failed to create AegisWeaponProjectile. Check if the prefab is set correctly in ProjectileFactory.");
            }
        }
        _durationTimer = _stats.AttackDuration;

        //if (!string.IsNullOrEmpty(SoundName)) SoundManager.instance.PlaySound(SoundName);
    }

    public override void ReleaseProjectile()
    {
        for (int i = 0; i < _activeProjectiles.Count; i++)
        {
            if (_activeProjectiles[i] != null) _activeProjectiles[i].StartShrink();
        }
        _activeProjectiles.Clear();
    }

    public override void Dead()
    {
        if (_isShrinking) return;
        ReleaseProjectile();
        _isShrinking = true;
        _shrinkTimer = 0f;
    }
}
